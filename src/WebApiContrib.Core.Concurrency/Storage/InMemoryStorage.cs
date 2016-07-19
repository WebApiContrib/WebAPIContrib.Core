using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace WebApiContrib.Core.Concurrency.Storage
{
    internal class InMemoryStorage : IStorage
    {
        private readonly IMemoryCache _memoryCache;

        public InMemoryStorage()
        {
            var builder = new ServiceCollection();
            builder.AddMemoryCache();
            var provider = builder.BuildServiceProvider();
            _memoryCache = (IMemoryCache)provider.GetService(typeof(IMemoryCache));
        }

        public ConcurrentObject TryGetValue(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            object value = null;
            if (!_memoryCache.TryGetValue(key, out value))
            {
                return null;
            }

            return value as ConcurrentObject;
        }

        public Task<ConcurrentObject> TryGetValueAsync(string key)
        {
            return Task.FromResult(TryGetValue(key));
        }

        public void Set(string key, ConcurrentObject value)
        {
            _memoryCache.Set(key, value);
        }

        public Task SetAsync(string key, ConcurrentObject value)
        {
            return Task.Factory.StartNew(() => Set(key, value));
        }

        public void Remove(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            _memoryCache.Remove(key);
        }

        public Task RemoveAsync(string key)
        {
            return Task.Factory.StartNew(() => Remove(key));
        }
    }
}
