using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Net.Http.Headers;

namespace WebApiContrib.Core.Formatter.Yaml
{
    public static class YamlFormatterMvcBuilderExtensions
    {
        public static IMvcBuilder AddYamlFormatters(this IMvcBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return AddYamlFormatters(builder, YamlFormatterOptionsConfiguration: null);
        }

        public static IMvcCoreBuilder AddYamlFormatters(this IMvcCoreBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return AddYamlFormatters(builder, YamlFormatterOptionsConfiguration: null);
        }

        public static IMvcBuilder AddYamlFormatters(this IMvcBuilder builder, Action<YamlFormatterOptions> YamlFormatterOptionsConfiguration)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var YamlFormatterOptions = new YamlFormatterOptions();
            YamlFormatterOptionsConfiguration?.Invoke(YamlFormatterOptions);

            foreach (var extension in YamlFormatterOptions.SupportedExtensions)
            {
                foreach (var contentType in YamlFormatterOptions.SupportedContentTypes)
                {
                    builder.AddFormatterMappings(m => m.SetMediaTypeMappingForFormat(extension, new MediaTypeHeaderValue(contentType)));
                }
            }

            builder.AddMvcOptions(options => options.InputFormatters.Add(new YamlInputFormatter(YamlFormatterOptions)));
            builder.AddMvcOptions(options => options.OutputFormatters.Add(new YamlOutputFormatter(YamlFormatterOptions)));

            return builder;
        }

        public static IMvcCoreBuilder AddYamlFormatters(this IMvcCoreBuilder builder, Action<YamlFormatterOptions> YamlFormatterOptionsConfiguration)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var YamlFormatterOptions = new YamlFormatterOptions();
            YamlFormatterOptionsConfiguration?.Invoke(YamlFormatterOptions);

            foreach (var extension in YamlFormatterOptions.SupportedExtensions)
            {
                foreach (var contentType in YamlFormatterOptions.SupportedContentTypes)
                {
                    builder.AddFormatterMappings(m => m.SetMediaTypeMappingForFormat(extension, new MediaTypeHeaderValue(contentType)));
                }
            }

            builder.AddMvcOptions(options => options.InputFormatters.Add(new YamlInputFormatter(YamlFormatterOptions)));
            builder.AddMvcOptions(options => options.OutputFormatters.Add(new YamlOutputFormatter(YamlFormatterOptions)));

            return builder;
        }
    }
}
