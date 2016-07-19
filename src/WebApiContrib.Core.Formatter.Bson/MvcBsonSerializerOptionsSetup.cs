using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;

namespace WebApiContrib.Core.Formatter.Bson
{
    public class MvcBsonSerializerOptionsSetup : ConfigureOptions<MvcOptions>
    {
        public MvcBsonSerializerOptionsSetup()
            : base(ConfigureMvc)
        {
        }        

        public static void ConfigureMvc(MvcOptions options)
        {
            options.OutputFormatters.Add(new BsonOutputFormatter(
                BsonSerializerSettingsProvider.CreateSerializerSettings()));
            options.InputFormatters.Add(new BsonInputFormatter(
                BsonSerializerSettingsProvider.CreateSerializerSettings(),
                new DefaultObjectPoolProvider()));
        }
    }
}
