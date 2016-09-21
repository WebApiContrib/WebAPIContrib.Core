using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;

namespace WebApiContrib.Core.Versioning
{
    /// <summary>
    /// A convenience base class for <c>Accept</c> header-based version strategies.
    /// </summary>
    public abstract class AcceptHeaderVersionStrategy : IVersionStrategy
    {
        /// <inheritdoc />
        int? IVersionStrategy.GetVersion(HttpContext context, RouteData routeData)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var headers = context.Request.GetTypedHeaders();

            var acceptHeaders = headers.Accept?.OrderByDescending(x => x.Quality ?? 1.0);

            if (acceptHeaders == null)
            {
                return null;
            }

            foreach (var acceptHeader in acceptHeaders)
            {
                var version = GetVersion(acceptHeader);

                if (version.HasValue)
                {
                    return version.Value;
                }
            }

            return null;
        }

        protected static string StripSuffix(string subType)
        {
            var suffixSeparatorIndex = subType.IndexOf('+');

            if (suffixSeparatorIndex >= 0)
            {
                return subType.Substring(0, suffixSeparatorIndex);
            }

            return subType;
        }

        /// <summary>
        /// Gets the currently requested resource version based on the given <see cref="MediaTypeHeaderValue"/>.
        /// </summary>
        /// <returns>A version integer, or <c>null</c> if a version cannot be determined.</returns>
        protected abstract int? GetVersion(MediaTypeHeaderValue acceptHeader);
    }
}