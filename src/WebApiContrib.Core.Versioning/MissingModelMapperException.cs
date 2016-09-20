using System;

namespace WebApiContrib.Core.Versioning
{
    /// <summary>
    /// This exception is thrown if <see cref="VersionNegotiationOptions.ThrowOnMissingMapper"/>
    /// is set to <c>true</c> and a mapper cannot be found for a specific model type.
    /// </summary>
    public class MissingModelMapperException : Exception
    {
        public MissingModelMapperException(Type modelType, Exception innerException)
            : base($"Could not find a registered mapper for '{modelType.FullName}'.", innerException)
        {
            ModelType = modelType;
        }

        public Type ModelType { get; }
    }
}