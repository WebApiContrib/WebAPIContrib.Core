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
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace WebApiContrib.Core.Concurrency.Storage
{
    public class BaseDistributedStorage : IStorage
    {
        private IDistributedCache _distributedCache;

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

        #region Protected methods

        protected void Initialize(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        #endregion
    }
}
