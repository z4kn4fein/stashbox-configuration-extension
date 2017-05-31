using System;
using System.Reflection;
using Stashbox.Utils;

namespace Stashbox.Configuration.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SettingAttribute : Attribute
    {
        public MethodInfo ConvertMethodInfo { get; private set; }

        public object Converter { get; private set; }

        public string Key { get; private set; }

        public SettingAttribute(string key, Type converterType = null)
        {
            Shield.EnsureNotNullOrEmpty(key, nameof(key));

            var convertMethodInfo = converterType?.GetMethod("Convert");
            if (convertMethodInfo != null)
            {
                this.ConvertMethodInfo = convertMethodInfo;
                this.Converter = Activator.CreateInstance(converterType);
            }

            this.Key = key;
        }
    }
}
