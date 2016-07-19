using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using Microsoft.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Bson;

namespace WebApiContrib.Core.Formatter.Bson
{
    /// <summary>
    /// A <see cref="BsonOutputFormatter"/> for BSON content
    /// </summary>
    public class BsonOutputFormatter : TextOutputFormatter
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        private JsonSerializer _serializer;
        
        public BsonOutputFormatter(JsonSerializerSettings serializerSettings)
        {
            if (serializerSettings == null)
            {
                throw new ArgumentNullException(nameof(serializerSettings));
            }


            _jsonSerializerSettings = serializerSettings;
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/bson"));
        }
        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (selectedEncoding == null)
            {
                throw new ArgumentNullException(nameof(selectedEncoding));
            }

            var response = context.HttpContext.Response;
            using (var bsonWriter = new BsonWriter(response.Body) { CloseOutput = false })
            {
                var jsonSerializer = CreateJsonSerializer();
                jsonSerializer.Serialize(bsonWriter, context.Object);
                bsonWriter.Flush();
            }
        }
        
        private JsonSerializer CreateJsonSerializer()
        {
            if (_serializer == null)
            {
                _serializer = JsonSerializer.Create(_jsonSerializerSettings);
            }

            return _serializer;
        }
    }
}
