using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Text;
using System.Threading.Tasks;

namespace WebApiContrib.Core.Formatter.Bson
{
    /// <summary>
    /// A <see cref="BsonInputFormatter"/> for BSON content
    /// </summary>
    public class BsonInputFormatter : TextInputFormatter
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        private readonly ObjectPoolProvider _objectPoolProvider;

        private ObjectPool<JsonSerializer> _jsonSerializerPool;

        public BsonInputFormatter(
            JsonSerializerSettings jsonSerializerSettings,
            ObjectPoolProvider objectPoolProvider)
        {
            _jsonSerializerSettings = jsonSerializerSettings;
            _objectPoolProvider = objectPoolProvider;
            SupportedEncodings.Add(Encoding.Unicode);
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/bson"));
        }
        
        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            var request = context.HttpContext.Request;
            using (var reader = new BsonReader(request.Body))
            {
               reader.ReadRootValueAsArray = IsEnumerable(context.ModelType);

                var successful = true;
                EventHandler<ErrorEventArgs> errorHandler = (sender, eventArgs) =>
                {
                    successful = false;
                    var exception = eventArgs.ErrorContext.Error;
                    eventArgs.ErrorContext.Handled = true;
                };                
                var jsonSerializer = CreateJsonSerializer();
                jsonSerializer.Error += errorHandler;
                var type = context.ModelType;
                object model;
                try
                {
                    model = jsonSerializer.Deserialize(reader, type);
                }
                finally
                {
                    jsonSerializer.Error -= errorHandler;
                    _jsonSerializerPool.Return(jsonSerializer);
                }

                if (successful)
                {
                    return InputFormatterResult.SuccessAsync(model);
                }

                return InputFormatterResult.FailureAsync();
            }
        }
        
        private JsonSerializer CreateJsonSerializer()
        {
            if (_jsonSerializerPool == null)
            {
                _jsonSerializerPool = _objectPoolProvider.Create(new BsonSerializerObjectPolicy(_jsonSerializerSettings));
            }

            return _jsonSerializerPool.Get();
        }

        private bool IsEnumerable(Type type)
        {
           return type.GetInterface(nameof(IEnumerable)) != null;
        }
   }
}
