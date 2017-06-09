using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Stashbox.Configuration.Attributes;
using Stashbox.Entity;
using Stashbox.Infrastructure;
using Stashbox.Infrastructure.ContainerExtension;
using Stashbox.Infrastructure.Registration;
using SettingAttribute = Stashbox.Configuration.Attributes.SettingAttribute;

namespace Stashbox.Configuration
{
    public class AutoConfigurationExtension : IRegistrationExtension, IPostBuildExtension
    {
        private readonly Func<string, string> settingReader;
        private readonly Func<string, string> connectionStringReader;
        private readonly string separator;
        private readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, PropertySetterInfo>> propertySetters;
        private readonly ConcurrentDictionary<Type, string> prefixes;

        public AutoConfigurationExtension(Func<string, string> settingReader = null, Func<string, string> connectionStringReader = null, string separator = ":")
        {
            this.settingReader = settingReader ?? (key => ConfigurationManager.AppSettings.Get(key));
            this.connectionStringReader = connectionStringReader ?? (key => ConfigurationManager.ConnectionStrings[key].ConnectionString);
            this.separator = separator;
            this.prefixes = new ConcurrentDictionary<Type, string>();
            this.propertySetters = new ConcurrentDictionary<Type, ConcurrentDictionary<string, PropertySetterInfo>>();
        }

        public IContainerExtension CreateCopy() =>
            new AutoConfigurationExtension(this.settingReader, this.connectionStringReader, this.separator);

        public void OnRegistration(IContainerContext containerContext, IServiceRegistration registration)
        {
            registration.ImplementationType.GetCustomSettingAttributes<SettingPrefixAttribute>()
                .ForEach(attribute => this.prefixes.AddOrUpdate(registration.ImplementationType, attribute.Prefix, (key, old) => attribute.Prefix));

            registration.ImplementationType.GetProperties().ForEach(property =>
            {
                property.GetCustomSettingAttributes<SettingAttribute>()
                    .ForEach(attribute => this.AddAttributeData(attribute, registration.ImplementationType, property, ConfigurationType.AppSetting));

                property.GetCustomSettingAttributes<ConnectionStringAttribute>()
                    .ForEach(attribute => this.AddAttributeData(attribute, registration.ImplementationType, property, ConfigurationType.ConnectionString));
            });
        }

        public object PostBuild(object instance, IContainerContext containerContext, ResolutionInfo resolutionInfo, IServiceRegistration serviceRegistration, Type requestedType)
        {
            string prefix;
            ConcurrentDictionary<string, PropertySetterInfo> setters;

            this.prefixes.TryGetValue(serviceRegistration.ImplementationType, out prefix);
            this.propertySetters.TryGetValue(serviceRegistration.ImplementationType, out setters);

            if (prefix == null && setters == null)
                return instance;

            setters.ForEach(setter =>
            {
                var key = prefix == null ? setter.Key : $"{prefix}{this.separator}{setter.Key}";
                var value = setter.Value.ConfigurationType == ConfigurationType.AppSetting ?
                    this.settingReader(key) : this.connectionStringReader(key);

                if (setter.Value.ConvertMethod != null && setter.Value.Converter != null)
                {
                    var conv = setter.Value.ConvertMethod.Invoke(setter.Value.Converter, new object[] { value });
                    setter.Value.SetterMethod.Invoke(instance, new[] { conv });
                }
                else if (setter.Value.PropertyType == typeof(string))
                    setter.Value.SetterMethod.Invoke(instance, new object[] { value });
                else
                {
                    var converter = TypeDescriptor.GetConverter(setter.Value.PropertyType);
                    var conv = converter.ConvertFromInvariantString(value);
                    setter.Value.SetterMethod.Invoke(instance, new[] { conv });
                }
            });

            return instance;
        }

        public void Initialize(IContainerContext containerContext)
        { }

        public void CleanUp()
        { }

        private void AddAttributeData(SettingAttribute attribute, Type typeTo, PropertyInfo property, ConfigurationType configurationType)
        {
            var item = this.propertySetters.GetOrAdd(typeTo, type => new ConcurrentDictionary<string, PropertySetterInfo>());

            var setter = new PropertySetterInfo
            {
                ConfigurationType = configurationType,
                ConvertMethod = attribute.ConvertMethodInfo,
                SetterMethod = property.GetSetMethod(),
                Converter = attribute.Converter,
                PropertyType = property.PropertyType
            };

            item.AddOrUpdate(attribute.Key, setter, (s, info) => setter);
        }
    }
}
