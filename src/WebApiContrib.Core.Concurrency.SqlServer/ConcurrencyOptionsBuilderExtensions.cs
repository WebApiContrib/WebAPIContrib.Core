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

using Microsoft.Extensions.Caching.SqlServer;
using System;

namespace WebApiContrib.Core.Concurrency.SqlServer
{
    public static class ConcurrencyOptionsBuilderExtensions
    {
        #region Public static methods

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

        #endregion
    }
}
