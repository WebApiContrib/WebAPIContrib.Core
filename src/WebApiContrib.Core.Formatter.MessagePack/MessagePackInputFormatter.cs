using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using MessagePack;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.WebUtilities;
using System.Threading;

namespace WebApiContrib.Core.Formatter.MessagePack
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

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var request = context.HttpContext.Request;

            if (!request.Body.CanSeek && !_options.SuppressReadBufferring)
            {
                BufferingHelper.EnableRewind(request);

                await request.Body.DrainAsync(CancellationToken.None);
                request.Body.Seek(0L, SeekOrigin.Begin);
            }

            var result = MessagePackSerializer.NonGeneric.Deserialize(context.ModelType, request.Body, _options.FormatterResolver);
            var formatterResult = await InputFormatterResult.SuccessAsync(result);

            return formatterResult;
        }

        public override bool CanRead(InputFormatterContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.ModelType == null)
            {
                throw new ArgumentException("Model Type cannnot be null");
            }

            var typeInfo = context.ModelType.GetTypeInfo();
            return !typeInfo.IsAbstract && !typeInfo.IsInterface && typeInfo.IsPublic;
        }
    }
}
