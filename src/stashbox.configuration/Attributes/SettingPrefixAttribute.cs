using System;
using Stashbox.Utils;

namespace Stashbox.Configuration.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SettingPrefixAttribute : Attribute
    {
        public string Prefix { get; private set; }
        public SettingPrefixAttribute(string prefix)
        {
            Shield.EnsureNotNullOrEmpty(prefix, nameof(prefix));

            this.Prefix = prefix;
        }
    }
}
