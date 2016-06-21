using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace WebApiContrib.Core.Formatter.PlainText
{
    public class PlainTextInputFormatter : TextInputFormatter
    {
        public PlainTextInputFormatter(PlainTextFormatterOptions opts)
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

        public PlainTextInputFormatter() : this(new PlainTextFormatterOptions())
        {
        }

        protected override bool CanReadType(Type type)
        {
            return typeof(string) == type;
        }

        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            var request = context.HttpContext.Request;
            string value;
            using (var reader = new StreamReader(request.Body))
            {
                value = reader.ReadToEnd();
            }

            return InputFormatterResult.SuccessAsync(value);
        }
    }
}