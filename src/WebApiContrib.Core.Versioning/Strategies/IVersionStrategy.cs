using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace WebApiContrib.Core.Versioning
{
    /// <summary>
    /// The interface for a version strategy for HTTP requests.
    /// </summary>
    public interface IVersionStrategy
    {
        /// <summary>
        /// Gets the currently requested resource version based on
        /// the given <see cref="HttpContext"/> and <see cref="RouteData"/>.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> of the current request.</param>
        /// <param name="routeData">The <see cref="RouteData"/>of the current request.</param>
        /// <returns>A version integer, or <c>null</c> if a version cannot be determined.</returns>
        VersionContext GetVersion(HttpContext context, RouteData routeData);
    }
}