using System;

namespace Stashbox.Configuration.Attributes
{
    /// <summary>
    /// Represents a connection string setting.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ConnectionStringAttribute : SettingAttribute
    {
        /// <summary>
        /// Constructs a <see cref="ConnectionStringAttribute"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        public ConnectionStringAttribute(string key)
            : base(key)
        { }
    }
}
