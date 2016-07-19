using Microsoft.Extensions.Caching.SqlServer;
using System;

namespace WebApiContrib.Core.Concurrency.SqlServer
{
    public static class ConcurrencyOptionsBuilderExtensions
    {
        public static void UseSqlServer(
            this ConcurrencyOptionsBuilder concurrencyOptionsBuilder,
            Action<SqlServerCacheOptions> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            var options = new SqlServerCacheOptions();
            callback(options);
            UseSqlServer(concurrencyOptionsBuilder, options);
        }

        public static void UseSqlServer(
            this ConcurrencyOptionsBuilder concurrencyOptionsBuilder,
            SqlServerCacheOptions options)
        {
            if (concurrencyOptionsBuilder == null)
            {
                throw new ArgumentNullException(nameof(concurrencyOptionsBuilder));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            concurrencyOptionsBuilder.ConcurrencyOptions.Storage = new SqlServerStorage(options);
        }
    }
}
