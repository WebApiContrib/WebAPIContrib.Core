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

using Microsoft.Extensions.Caching.Redis;
using System;

namespace WebApiContrib.Core.Concurrency.Redis
{
    public static class ConcurrencyOptionsBuilderExtensions
    {
        #region Public static methods

        public static void UseRedis(
            this ConcurrencyOptionsBuilder concurrencyOptionsBuilder,
            Action<RedisCacheOptions> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            var options = new RedisCacheOptions();
            callback(options);
            UseRedis(concurrencyOptionsBuilder, options);
        }

        public static void UseRedis(
            this ConcurrencyOptionsBuilder concurrencyOptionsBuilder,
            RedisCacheOptions options)
        {
            if (concurrencyOptionsBuilder == null)
            {
                throw new ArgumentNullException(nameof(concurrencyOptionsBuilder));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            concurrencyOptionsBuilder.ConcurrencyOptions.Storage = new RedisStorage(options);
        }

        #endregion
    }
}
