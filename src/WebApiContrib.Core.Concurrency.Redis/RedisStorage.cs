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

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using WebApiContrib.Core.Concurrency.Storage;

namespace WebApiContrib.Core.Concurrency.Redis
{
    internal class RedisStorage : BaseDistributedStorage
    {
        #region Constructor

        public RedisStorage(RedisCacheOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var builder = new ServiceCollection();
            builder.AddSingleton<IDistributedCache>(serviceProvider =>
            new RedisCache(Options.Create(options)));
            var provider = builder.BuildServiceProvider();
            Initialize((IDistributedCache)provider.GetService(typeof(IDistributedCache)));
        }

        #endregion
    }
}
