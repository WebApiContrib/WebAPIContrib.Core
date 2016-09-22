using System;

namespace WebApiContrib.Core.Versioning
{
    /// <summary>
    /// This exception is thrown if <see cref="VersionNegotiationOptions.ThrowOnMissingMapper"/>
    /// is set to <c>true</c> and a mapper cannot be found for a specific model type.
    /// </summary>
    public class MissingModelMapperException : Exception
    {
        /// <summary>
        /// Creates an instance of <see cref="MissingModelMapperException"/>.
        /// </summary>
        /// <param name="modelType">The type of model that does not have a registered mapper.</param>
        /// <param name="innerException">The inner exception.</param>
        public MissingModelMapperException(Type modelType, Exception innerException)
            : base($"Could not find a registered mapper for '{modelType.FullName}'.", innerException)
        {
            ModelType = modelType;
        }

        /// <summary>
        /// Gets the type of model that does not have a registered mapper.
        /// </summary>
        public Type ModelType { get; }
    }
}