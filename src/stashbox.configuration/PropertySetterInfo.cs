using System;
using System.Reflection;

namespace Stashbox.Configuration
{
    internal enum ConfigurationType
    {
        AppSetting,
        ConnectionString
    }

    internal class PropertySetterInfo
    {
        public MethodInfo SetterMethod { get; set; }

        public ConfigurationType ConfigurationType { get; set; }

        public Type PropertyType { get; set; }

        public MethodInfo ConvertMethod { get; set; }

        public object Converter { get; set; }
    }
}
