using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;

namespace WebApiContrib.Core.Versioning
{
    public class CustomHeaderVersionStrategy : IVersionStrategy
    {
        public string HeaderName { get; set; } = "Api-Version";

        int? IVersionStrategy.GetVersion(HttpContext context, RouteData routeData)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var headers = context.Request.Headers;

            StringValues header;
            if (headers.TryGetValue(HeaderName, out header))
            {
                int? version;
                if (ParsingUtility.TryParseVersion(header.ToString(), out version))
                {
                    return version;
                }
            }

            return null;
        }
    }
}