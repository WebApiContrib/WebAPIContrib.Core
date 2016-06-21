using System;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace WebApiContrib.Core.Formatter.PlainText
{
    public class PlainTextOutputFormatter : StringOutputFormatter
    {
        public PlainTextOutputFormatter(PlainTextFormatterOptions opts)
        {
            foreach (var enc in opts.SupportedEncodings)
            {
                SupportedEncodings.Add(enc);
            }

            foreach (var mediaType in opts.SupportedMediaTypes)
            {
                SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(mediaType));
            }
        }

        public PlainTextOutputFormatter() : this(new PlainTextFormatterOptions())
        {
        }

        public override bool CanWriteResult(OutputFormatterCanWriteContext context)
        {
            // this override is necessary to stop StringOutputFormatter from injecting UTF-8
            // and text/plain media type arbitrarily
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return context.ObjectType == typeof(string) || context.Object is string;
        }
    }
}