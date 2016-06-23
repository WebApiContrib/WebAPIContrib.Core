using System;
using System.ComponentModel;
using System.Reflection;

namespace WebApiContrib.Core.Internal
{
    public static class TypeExtensions
    {
        public static bool CanBeConvertedFromString(this Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;
            return type.GetTypeInfo().IsPrimitive ||
                   type == typeof(string) ||
                   type == typeof(DateTime) ||
                   type == typeof(DateTimeOffset) ||
                   Convert.GetTypeCode(type) != TypeCode.Object ||
                   TypeDescriptor.GetConverter(type).CanConvertFrom(typeof(string));
        }
    }
}
