using System;
using System.Linq;
using System.Reflection;

namespace Utils
{
    public static class ReflectionUtils
    {
        /// <summary>
        /// Gets all constant values of type T from the specified static class.
        /// </summary>
        public static T[] GetConstantValues<T>(Type type)
        {
            return type
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(T))
                .Select(f => (T) f.GetRawConstantValue())
                .ToArray();
        }

        /// <summary>
        /// Gets all static readonly values of type T from the specified static class.
        /// </summary>
        public static T[] GetStaticReadonlyValues<T>(Type type)
        {
            return type
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(f => f.IsInitOnly && f.FieldType == typeof(T))
                .Select(f => (T) f.GetValue(null))
                .ToArray();
        }
    }
}