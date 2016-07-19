using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Linq;

namespace WebApiContrib.Core.Concurrency.Extensions
{
    public static class ControllerExtensions
    {
       
        public static string GetIfMatch(this Controller controller)
        {
            return Get(controller.Request.Headers, Constants.ConcurrencyParameterNames.IfMatch);
        }

        public static string GetIfNoneMatch(this Controller controller)
        {
            return Get(controller.Request.Headers, Constants.ConcurrencyParameterNames.IfNoneMatch);
        }

        public static string GetUnmodifiedSince(this Controller controller)
        {
            return Get(controller.Request.Headers, Constants.ConcurrencyParameterNames.IfUnmodifiedSince);
        }

        public static string GetModifiedSince(this Controller controller)
        {
            return Get(controller.Request.Headers, Constants.ConcurrencyParameterNames.IfModifiedSince);
        }

        public static void SetEtag(this Controller controller, string value)
        {
            controller.Response.Headers.Add(Constants.ConcurrencyParameterNames.Etag, value);
        }

        public static void SetLastModifiedDate(this Controller controller, string value)
        {
            controller.Response.Headers.Add(Constants.ConcurrencyParameterNames.LastModified, value);
        }      

        private static string Get(this IHeaderDictionary headers, string key)
        {
            var values = new StringValues();
            if (!headers.TryGetValue(key, out values))
            {
                return null;
            }

            return values.FirstOrDefault();
        }
    }
}
