using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using YamlDotNet.Serialization;

namespace WebApiContrib.Core.Formatter.Yaml
{
    public class YamlOutputFormatter :  OutputFormatter
    {
        private readonly YamlFormatterOptions _options;
        private readonly Serializer _serializer;

        public YamlOutputFormatter(YamlFormatterOptions YamlFormatterOptions)
        {
            ContentType = "application/x-yaml";
            _options = YamlFormatterOptions ?? throw new ArgumentNullException(nameof(YamlFormatterOptions));
            foreach (var contentType in YamlFormatterOptions.SupportedContentTypes)
            {
                SupportedMediaTypes.Add(new MediaTypeHeaderValue(contentType));
            }
            _serializer = new Serializer();
        }

        public string ContentType { get; private set; }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var response = context.HttpContext.Response;

            using (var streamWriter = new StreamWriter(response.Body))
            {
                _serializer.Serialize(streamWriter, context.Object);
                return Task.FromResult(response);
            }
        }
    }
}
