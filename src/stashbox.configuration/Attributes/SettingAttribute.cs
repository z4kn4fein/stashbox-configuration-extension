using Stashbox.Utils;
using System;
using System.Reflection;

namespace Stashbox.Configuration.Attributes
{
    /// <summary>
    /// Represents an attribute which identifies a setting.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SettingAttribute : Attribute
    {
        /// <summary>
        /// Custom method which handles the conversion.
        /// </summary>
        public MethodInfo ConvertMethodInfo { get; private set; }

        /// <summary>
        /// Custom converter object.
        /// </summary>
        public object Converter { get; private set; }

        /// <summary>
        /// The setting key.
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Constructs a <see cref="SettingAttribute"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="converterType">The converter type.</param>
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
