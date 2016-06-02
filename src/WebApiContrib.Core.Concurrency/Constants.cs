#region copyright
// Copyright 2016 WebApiContrib
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

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
