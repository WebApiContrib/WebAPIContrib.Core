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
using System.Linq;
using System.Net.Http.Headers;

namespace WebApiContrib.Core.Concurrency.Extensions
{
    public static class HttpResponseHeaderExtensions
    {
        #region Public static methods

        public static string GetEtag(this HttpResponseHeaders headers)
        {
            return Get(headers, Constants.ConcurrencyParameterNames.Etag);
        }

        public static string GetModifiedDate(this HttpResponseHeaders headers)
        {
            return Get(headers, Constants.ConcurrencyParameterNames.LastModified);
        }

        #endregion

        #region Private static methods

        private static string Get(this HttpResponseHeaders headers, string key)
        {
            IEnumerable<string> values = new List<string>();
            if (!headers.TryGetValues(key, out values))
            {
                return null;
            }

            return values.FirstOrDefault();
        }

        #endregion
    }
}
