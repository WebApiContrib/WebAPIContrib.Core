using Microsoft.AspNetCore.Mvc;

namespace WebApiContrib.Core.Versioning
{
    /// <summary>
    /// An <see cref="ObjectResult"/> used to opt-in to versioning.
    /// To use regular <see cref="ObjectResult"/>s for versioning, you can set
    /// <see cref="VersionNegotiationOptions.RequireVersionedObjectResult"/> to <c>false</c>.
    /// </summary>
    public class VersionedObjectResult : ObjectResult
    {
        /// <summary>
        /// Creates an instance of <see cref="VersionedObjectResult"/>.
        /// </summary>
        /// <param name="value">The result value.</param>
        public VersionedObjectResult(object value) : base(value)
        {
        }
    }
}