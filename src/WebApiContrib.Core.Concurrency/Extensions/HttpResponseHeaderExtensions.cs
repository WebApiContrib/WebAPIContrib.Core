using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace WebApiContrib.Core.Concurrency.Extensions
{
    public static class HttpResponseHeaderExtensions
    {
        public static string GetEtag(this HttpResponseHeaders headers)
        {
            return Get(headers, Constants.ConcurrencyParameterNames.Etag);
        }

        public static string GetModifiedDate(this HttpResponseHeaders headers)
        {
            return Get(headers, Constants.ConcurrencyParameterNames.LastModified);
        }

        private static string Get(this HttpResponseHeaders headers, string key)
        {
            IEnumerable<string> values = new List<string>();
            if (!headers.TryGetValues(key, out values))
            {
                return null;
            }

            return values.FirstOrDefault();
        }
    }
}
