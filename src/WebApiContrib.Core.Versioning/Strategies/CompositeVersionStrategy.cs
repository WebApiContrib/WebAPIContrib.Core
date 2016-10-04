using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;

namespace WebApiContrib.Core.Versioning
{
    internal class CompositeVersionStrategy : IVersionStrategy
    {
        public CompositeVersionStrategy(IEnumerable<IVersionStrategy> versionStrategies)
        {
            VersionStrategies = versionStrategies;
        }

        private IEnumerable<IVersionStrategy> VersionStrategies { get; }

        public VersionResult? GetVersion(HttpContext context, RouteData routeData)
        {
            foreach (var strategy in VersionStrategies)
            {
                var version = strategy.GetVersion(context, routeData);

                if (version != null)
                {
                    return version;
                }
            }

            return null;
        }
    }
}
