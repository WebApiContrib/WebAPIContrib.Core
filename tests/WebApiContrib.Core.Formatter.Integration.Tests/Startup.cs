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

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApiContrib.Core.Concurrency.Extensions;
using WebApiContrib.Core.Concurrency.SqlServer;
using WebApiContrib.Core.Concurrency.Redis;

namespace WebApiContrib.Core.Concurrency.Integration.Tests
{
    public class Startup
    {
        #region Public methods

        public IConfigurationRoot Configuration { get; set; }

        #endregion

        #region Private methods

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // InMemory Storage
            // services.AddConcurrency(opt => opt.UseInMemoryStorage());
            // SqlServer storage
            /*
            services.AddConcurrency(opt => opt.UseSqlServer(o =>
            {
                o.ConnectionString = "Data Source=.;Initial Catalog=Caching;User Id=user;Password=Password19890;";
                o.SchemaName = "dbo";
                o.TableName = "MyAppCache";
            }));
            */
            // Redis storage
            // Install redis on your machine + run the command redis-server + run command redis-cli
            services.AddConcurrency(opt => opt.UseRedis(o =>
            {
                o.Configuration = "localhost";
                o.InstanceName = "SampleInstance";
            }));
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            app.UseMvc();
        }

        #endregion
    }
}
