using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace WebApiContrib.Core.Versioning
{
    /// <summary>
    /// This versioning strategy gets its version from the
    /// resource URI, using a captured route value.
    /// </summary>
    public class RouteValueVersionStrategy : IVersionStrategy
    {
        /// <summary>
        /// The name of the route value to check.
        /// </summary>
        public string RouteValueKey { get; set; } = "version";

        /// <inheritdoc />
        VersionResult? IVersionStrategy.GetVersion(HttpContext context, RouteData routeData)
        {
            if (routeData == null)
            {
                throw new ArgumentNullException(nameof(routeData));
            }

            var routeValues = routeData.Values;

            if (!routeValues.TryGetValue(RouteValueKey, out var versionObject))
            {
                return null;
            }

            var versionString = versionObject as string;

            if (ParsingUtility.TryParseVersion(versionString, out var version))
            {
                return new VersionResult(version);
            }

            return null;
        }
    }
}
