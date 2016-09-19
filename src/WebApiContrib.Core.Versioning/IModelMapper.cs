namespace WebApiContrib.Core.Versioning
{
    /// <summary>
    /// The interface for mapping an model to a versioned representation.
    /// </summary>
    /// <typeparam name="TModel">The model type to map.</typeparam>
    public interface IModelMapper<in TModel>
    {
        /// <summary>
        /// Maps the <paramref name="model"/> to a versioned representation using <paramref name="version"/>.
        /// </summary>
        /// <param name="model">The model to map.</param>
        /// <param name="version">The version, or <c>null</c> if a version could not be determined.</param>
        /// <returns>The versioned representation of the model.</returns>
        object Map(TModel model, int? version);
    }
}