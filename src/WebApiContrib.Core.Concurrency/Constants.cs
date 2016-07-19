using System.Collections.Generic;

namespace WebApiContrib.Core.Concurrency
{
    internal static class Constants
    {
        public static class HttpMethodNames
        {
            public const string PostMethod = "POST";

            public const string PutMethod = "PUT";

            public const string DeleteMethod = "DELETE";

            public const string GetMethod = "GET";

            public const string HeadMethod = "HEAD";
        }

        public static class ConcurrencyParameterNames
        {
            public const string LastModified = "Last-Modified";

            public const string Etag = "ETag";

            public const string IfMatch = "If-Match";

            public const string IfNoneMatch = "If-None-Match";

            public const string IfModifiedSince = "If-Modified-Since";

            public const string IfUnmodifiedSince = "If-Unmodified-Since";

            public const string IfRange = "If-Range";
        }

        public static List<string> StateChangingMethods = new List<string>
        {
            HttpMethodNames.PostMethod,
            HttpMethodNames.PutMethod,
            HttpMethodNames.DeleteMethod
        };

        public static List<string> ConsultationMethods = new List<string>
        {
            HttpMethodNames.GetMethod,
            HttpMethodNames.HeadMethod
        };
    }
}
