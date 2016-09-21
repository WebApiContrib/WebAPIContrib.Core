using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;

namespace WebApiContrib.Core.Versioning
{
    internal class CompositeVersionStrategy : IVersioningStrategy
    {
        public CompositeVersionStrategy(IEnumerable<IVersioningStrategy> versionStrategies)
        {
            VersionStrategies = versionStrategies;
        }

        public IEnumerable<IVersioningStrategy> VersionStrategies { get; }

        public int? GetVersion(HttpContext context, RouteData routeData)
        {
            foreach (var strategy in VersionStrategies)
            {
                var version = strategy.GetVersion(context, routeData);

                if (version.HasValue)
                {
                    return version;
                }
            }

            return null;
        }
    }
}
