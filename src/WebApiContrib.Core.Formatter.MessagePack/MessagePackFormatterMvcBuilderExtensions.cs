using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Net.Http.Headers;

namespace WebApiContrib.Core.Formatter.MessagePack
{
    public static class MessagePackFormatterMvcBuilderExtensions
    {
        public static IMvcBuilder AddMessagePackFormatters(this IMvcBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return AddMessagePackFormatters(builder, messagePackFormatterOptionsConfiguration: null);
        }

        public static IMvcCoreBuilder AddMessagePackFormatters(this IMvcCoreBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return AddMessagePackFormatters(builder, messagePackFormatterOptionsConfiguration: null);
        }

        public static IMvcBuilder AddMessagePackFormatters(this IMvcBuilder builder, Action<MessagePackFormatterOptions> messagePackFormatterOptionsConfiguration)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var messagePackFormatterOptions = new MessagePackFormatterOptions();
            messagePackFormatterOptionsConfiguration?.Invoke(messagePackFormatterOptions);

            foreach (var extension in messagePackFormatterOptions.SupportedExtensions)
            {
                foreach (var contentType in messagePackFormatterOptions.SupportedContentTypes)
                {
                    builder.AddFormatterMappings(m => m.SetMediaTypeMappingForFormat(extension, new MediaTypeHeaderValue(contentType)));
                }
            }

            builder.AddMvcOptions(options => options.InputFormatters.Add(new MessagePackInputFormatter(messagePackFormatterOptions)));
            builder.AddMvcOptions(options => options.OutputFormatters.Add(new MessagePackOutputFormatter(messagePackFormatterOptions)));

            return builder;
        }

        public static IMvcCoreBuilder AddMessagePackFormatters(this IMvcCoreBuilder builder, Action<MessagePackFormatterOptions> messagePackFormatterOptionsConfiguration)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var messagePackFormatterOptions = new MessagePackFormatterOptions();
            messagePackFormatterOptionsConfiguration?.Invoke(messagePackFormatterOptions);

            foreach (var extension in messagePackFormatterOptions.SupportedExtensions)
            {
                foreach (var contentType in messagePackFormatterOptions.SupportedContentTypes)
                {
                    builder.AddFormatterMappings(m => m.SetMediaTypeMappingForFormat(extension, new MediaTypeHeaderValue(contentType)));
                }
            }

            builder.AddMvcOptions(options => options.InputFormatters.Add(new MessagePackInputFormatter(messagePackFormatterOptions)));
            builder.AddMvcOptions(options => options.OutputFormatters.Add(new MessagePackOutputFormatter(messagePackFormatterOptions)));

            return builder;
        }
    }
}
