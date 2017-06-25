using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Net.Http.Headers;

namespace WebApiContrib.Core.Formatter.Csv
{
    public static class MessagePackFormatterMvcBuilderExtensions
    {
        public static IMvcBuilder AddMessagePackFormatters(this IMvcBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return AddMessagePackFormatters(builder, messagePackFormatterOptions: null);
        }

        public static IMvcBuilder AddMessagePackFormatters( this IMvcBuilder builder, MessagePackFormatterOptions messagePackFormatterOptions)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddFormatterMappings(m => m.SetMediaTypeMappingForFormat("mp", new MediaTypeHeaderValue("application/x-msgpack")));

            if (messagePackFormatterOptions == null)
            {
                messagePackFormatterOptions = new MessagePackFormatterOptions();
            }

            builder.AddMvcOptions(options => options.InputFormatters.Add(new MessagePackInputFormatter(messagePackFormatterOptions)));
            builder.AddMvcOptions(options => options.OutputFormatters.Add(new MessagePackOutputFormatter(messagePackFormatterOptions)));


            return builder;
        }
    }
}
