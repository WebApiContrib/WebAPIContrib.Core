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

        #region Constructor

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

        #endregion

        #region Public methods

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

        #endregion

        #region Private methods

        private JsonSerializer CreateJsonSerializer()
        {
            if (_serializer == null)
            {
                _serializer = JsonSerializer.Create(_jsonSerializerSettings);
            }

            return _serializer;
        }

        #endregion
    }
}
