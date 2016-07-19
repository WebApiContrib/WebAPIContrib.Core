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
    }
}
