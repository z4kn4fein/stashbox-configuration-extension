using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Stashbox.Utils;

namespace System
{
    internal static class TypeExtensions
    {
        public static IEnumerable<TAttribute> GetCustomSettingAttributes<TAttribute>(this Type type) where TAttribute : Attribute =>
            type.GetCustomAttributes(typeof(TAttribute), true).OfType<TAttribute>();
    }
}

namespace System.Reflection
{
    internal static class PropertyInfoExtensions
    {
        public static IEnumerable<TAttribute> GetCustomSettingAttributes<TAttribute>(this PropertyInfo property) where TAttribute : Attribute =>
            property.GetCustomAttributes(typeof(TAttribute), true).OfType<TAttribute>();
    }
}

namespace System.Linq
{
    internal static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            Shield.EnsureNotNull(action, nameof(action));

            foreach (var item in enumerable)
                action(item);
        }
    }
}
