using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using YamlDotNet.Serialization;

namespace WebApiContrib.Core.Formatter.Yaml
{
    public class YamlInputFormatter : InputFormatter
    {
        private readonly YamlFormatterOptions _options;
        private readonly Deserializer _deserializer;

        public YamlInputFormatter(YamlFormatterOptions YamlFormatterOptions)
        {
            _options = YamlFormatterOptions ?? throw new ArgumentNullException(nameof(YamlFormatterOptions));
            foreach (var contentType in YamlFormatterOptions.SupportedContentTypes)
            {
                SupportedMediaTypes.Add(new MediaTypeHeaderValue(contentType));
            }
            _deserializer = new Deserializer();
        }

        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var type = context.ModelType;
            using (var streamReader = new StreamReader(context.HttpContext.Request.Body))
            {
                object result = _deserializer.Deserialize(streamReader, type);
                return InputFormatterResult.SuccessAsync(result);
            }
        }

        public override bool CanRead(InputFormatterContext context)
        {
            return true;
        }
    }
}
