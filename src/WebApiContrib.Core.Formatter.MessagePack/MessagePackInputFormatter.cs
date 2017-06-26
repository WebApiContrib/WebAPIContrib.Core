using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using MessagePack;

namespace WebApiContrib.Core.Formatter.Csv
{
    public class MessagePackInputFormatter : InputFormatter
    {
        private readonly MessagePackFormatterOptions _options;

        public MessagePackInputFormatter(MessagePackFormatterOptions messagePackFormatterOptions)
        {
            _options = messagePackFormatterOptions ?? throw new ArgumentNullException(nameof(messagePackFormatterOptions));
            foreach (var contentType in messagePackFormatterOptions.SupportedContentTypes)
            {
                SupportedMediaTypes.Add(new MediaTypeHeaderValue(contentType));
            }
        }

        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var request = context.HttpContext.Request;
            var result = MessagePackSerializer.NonGeneric.Deserialize(context.ModelType, request.Body, _options.FormatterResolver);
            return InputFormatterResult.SuccessAsync(result);
        }

        public override bool CanRead(InputFormatterContext context)
        {
            if (context.ModelType == null)
            {
                throw new ArgumentException("Model Type cannnot be null");
            }

            var typeInfo = context.ModelType.GetTypeInfo();
            return !typeInfo.IsAbstract && !typeInfo.IsInterface && typeInfo.IsPublic;
        }
    }
}
