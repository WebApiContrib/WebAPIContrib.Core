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
using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;
using WebApiContrib.Core.Concurrency.Storage;

namespace WebApiContrib.Core.Concurrency.SqlServer
{
    internal class SqlServerStorage : IStorage
    {
        private readonly IDistributedCache _distributedCache;

        #region Constructor

        public SqlServerStorage(SqlServerCacheOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var builder = new ServiceCollection();
            builder.AddSingleton<IDistributedCache>(serviceProvider =>
            new SqlServerCache(Options.Create(options)));
            var provider = builder.BuildServiceProvider();
            _distributedCache = (IDistributedCache)provider.GetService(typeof(IDistributedCache));
        }

        #endregion

        #region Public methods

        public void Remove(string key)
        {
            RemoveAsync(key).Wait();
        }

        public async Task RemoveAsync(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            await _distributedCache.RemoveAsync(key);
        }

        public void Set(string key, ConcurrentObject value)
        {
            SetAsync(key, value).Wait();
        }

        public async Task SetAsync(string key, ConcurrentObject value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var serializedObject = JsonConvert.SerializeObject(value);
            await _distributedCache.SetAsync(key.ToString(), Encoding.UTF8.GetBytes(serializedObject), new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.UtcNow.AddYears(2)
            });
        }

        public ConcurrentObject TryGetValue(string key)
        {
            throw new NotImplementedException();
        }

        public async Task<ConcurrentObject> TryGetValueAsync(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var bytes = await _distributedCache.GetAsync(key.ToString());
            if (bytes == null)
            {
                return null;
            }

            var serialized = Encoding.UTF8.GetString(bytes);
            return JsonConvert.DeserializeObject<ConcurrentObject>(serialized);
        }

        #endregion
    }
}
