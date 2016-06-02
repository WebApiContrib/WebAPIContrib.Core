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

using Microsoft.AspNetCore.Builder;

namespace WebApiContrib.Core.Concurrency.Extensions
{
    public static class MiddlewareExtensions
    {
        #region Public static methods

        public static IApplicationBuilder UseConcurrency(this IApplicationBuilder applicationBuilder)
        {
            // return applicationBuilder.UseMiddleware<ConcurrencyMiddleware>();
            return applicationBuilder;
        }

        #endregion
    }
}
