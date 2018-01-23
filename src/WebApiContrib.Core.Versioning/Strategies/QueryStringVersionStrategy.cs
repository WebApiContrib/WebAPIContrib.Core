using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace WebApiContrib.Core.Versioning
{
    /// <summary>
    /// This version strategy gets its version from a query string parameter.
    /// The parameter name can be configured using the <see cref="ParameterName"/> property.
    /// </summary>
    public class QueryStringVersionStrategy : IVersionStrategy
    {
        /// <summary>
        /// Gets or sets the parameter name to use for determining the version.
        /// </summary>
        /// <remarks>
        /// The default value is <c>version</c>.
        /// </remarks>
        public string ParameterName { get; set; } = "version";

        /// <inheritdoc />
        public VersionResult? GetVersion(HttpContext context, RouteData routeData)
        {
            if (context.Request.Query.TryGetValue(ParameterName, out var values))
            {
                var versionString = values.ToString();

                if (ParsingUtility.TryParseVersion(versionString, out var version))
                {
                    return new VersionResult(version);
                }
            }

            return null;
        }
    }
}
