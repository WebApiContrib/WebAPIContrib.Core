using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using MessagePack;
using Microsoft.Net.Http.Headers;

namespace WebApiContrib.Core.Formatter.MessagePack
{
    public class MessagePackOutputFormatter : OutputFormatter
    {
        private readonly MessagePackFormatterOptions _options;

        public MessagePackOutputFormatter(MessagePackFormatterOptions messagePackFormatterOptions)
        {
            _options = messagePackFormatterOptions ?? throw new ArgumentNullException(nameof(messagePackFormatterOptions));
            foreach (var contentType in messagePackFormatterOptions.SupportedContentTypes)
            {
                SupportedMediaTypes.Add(new MediaTypeHeaderValue(contentType));
            }
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            MessagePackSerializer.NonGeneric.Serialize(context.ObjectType, context.HttpContext.Response.Body, context.Object, _options.FormatterResolver);
            return Task.FromResult(0);
        }
    }
}
