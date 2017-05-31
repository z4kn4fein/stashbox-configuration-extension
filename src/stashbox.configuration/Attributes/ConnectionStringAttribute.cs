using System;

namespace Stashbox.Configuration.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ConnectionStringAttribute : SettingAttribute
    {
        public ConnectionStringAttribute(string key)
            : base(key)
        { }
    }
}
