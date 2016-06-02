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

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Linq;

namespace WebApiContrib.Core.Concurrency.Extensions
{
    public static class ControllerExtensions
    {
        #region Public static methods
        
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
        
        #endregion

        #region Private static methods

        private static string Get(this IHeaderDictionary headers, string key)
        {
            var values = new StringValues();
            if (!headers.TryGetValue(key, out values))
            {
                return null;
            }

            return values.FirstOrDefault();
        }
        #endregion
    }
}
