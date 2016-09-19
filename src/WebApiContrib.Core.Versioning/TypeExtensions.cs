using System;
using System.Reflection;

namespace WebApiContrib.Core.Versioning
{
    internal static class TypeExtensions
    {
        public static bool IsAssignableToGenericTypeDefinition(this Type type, Type genericTypeDefinition, out Type[] genericTypeArguments)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (genericTypeDefinition == null)
            {
                throw new ArgumentNullException(nameof(genericTypeDefinition));
            }

            var typeInfo = type.GetTypeInfo();

            // First, check all the implemented interfaces...
            foreach (var interfaceType in typeInfo.ImplementedInterfaces)
            {
                var interfaceTypeInfo = interfaceType.GetTypeInfo();

                if (interfaceTypeInfo.HasGenericTypeDefinition(genericTypeDefinition))
                {
                    genericTypeArguments = interfaceTypeInfo.GenericTypeArguments;
                    return true;
                }
            }

            // Then, check the type itself...
            if (typeInfo.HasGenericTypeDefinition(genericTypeDefinition))
            {
                genericTypeArguments = typeInfo.GenericTypeArguments;
                return true;
            }

            var baseType = typeInfo.BaseType;

            if (baseType == null)
            {
                genericTypeArguments = Type.EmptyTypes;
                return false;
            }

            // Finally, move up to the base class.
            return baseType.IsAssignableToGenericTypeDefinition(genericTypeDefinition, out genericTypeArguments);
        }

        public static bool HasGenericTypeDefinition(this TypeInfo typeInfo, Type genericTypeDefinition)
        {
            return typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == genericTypeDefinition;
        }
    }
}