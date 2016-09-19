namespace WebApiContrib.Core.Versioning
{
    public class VersionNegotiationOptions
    {
        public VersionNegotiationOptions()
        {
            RequireVersionedObjectResult = true;
            ThrowOnMissingMapper = true;
        }

        /// <summary>
        /// Gets or sets whether the user is required to explicitly return a
        /// <see cref="VersionedObjectResult"/> instead of a plain
        /// <see cref="Microsoft.AspNetCore.Mvc.ObjectResult"/> in
        /// order to negotiate a versioned representation.
        /// </summary>
        /// <remarks>
        /// The default is <c>true</c>.
        /// </remarks>
        public bool RequireVersionedObjectResult { get; set; }

        /// <summary>
        /// Gets or sets whether the version negotiation filter will throw a
        /// <see cref="MissingModelMapperException"/> or just return the
        /// result directly when a mapper cannot be found for the current result.
        /// </summary>
        /// <remarks>
        /// The default is <c>true</c>.
        /// </remarks>
        public bool ThrowOnMissingMapper { get; set; }
    }
}