using Stashbox.Utils;
using System;

namespace Stashbox.Configuration.Attributes
{
    /// <summary>
    /// Represents a setting prefix.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SettingPrefixAttribute : Attribute
    {
        /// <summary>
        /// The prefix.
        /// </summary>
        public string Prefix { get; private set; }

        /// <summary>
        /// Constructs a <see cref="SettingPrefixAttribute"/>.
        /// </summary>
        /// <param name="prefix"></param>
        public SettingPrefixAttribute(string prefix)
        {
            Shield.EnsureNotNullOrEmpty(prefix, nameof(prefix));

            this.Prefix = prefix;
        }
    }
}
