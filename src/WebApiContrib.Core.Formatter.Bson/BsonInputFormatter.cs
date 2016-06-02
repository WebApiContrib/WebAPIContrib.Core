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

using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Serialization;
using System;
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

        #region Constructor

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

        #endregion

        #region Public methods

        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            var request = context.HttpContext.Request;
            using (var reader = new BsonReader(request.Body))
            {
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
                    _jsonSerializerPool.Return(jsonSerializer);
                }

                if (successful)
                {
                    return InputFormatterResult.SuccessAsync(model);
                }

                return InputFormatterResult.FailureAsync();
            }
        }

        #endregion

        #region Private methods

        private JsonSerializer CreateJsonSerializer()
        {
            if (_jsonSerializerPool == null)
            {
                _jsonSerializerPool = _objectPoolProvider.Create(new BsonSerializerObjectPolicy(_jsonSerializerSettings));
            }

            return _jsonSerializerPool.Get();
        }

        #endregion
    }
}
