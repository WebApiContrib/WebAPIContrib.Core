using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using WebApiContrib.Core.Concurrency.Storage;

namespace WebApiContrib.Core.Concurrency.SqlServer
{
    internal class SqlServerStorage : BaseDistributedStorage
    {

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
            Initialize((IDistributedCache)provider.GetService(typeof(IDistributedCache)));
        }
    }
}
