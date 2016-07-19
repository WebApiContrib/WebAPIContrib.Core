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
        
        protected void Initialize(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
    }
}
