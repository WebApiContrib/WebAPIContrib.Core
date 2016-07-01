using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace WebApiContrib.Core.Formatter.Jsonp
{
    public class JsonpMediaTypeFormatter : TextOutputFormatter
    {
        private static readonly MediaTypeHeaderValue _applicationJavaScript = new MediaTypeHeaderValue("application/javascript");
        private static readonly MediaTypeHeaderValue _applicationJsonp = new MediaTypeHeaderValue("application/json-p");
        private static readonly MediaTypeHeaderValue _textJavaScript = new MediaTypeHeaderValue("text/javascript");
        private readonly JsonOutputFormatter _jsonMediaTypeFormatter;
        private readonly string _callbackQueryParameter;

        public JsonpMediaTypeFormatter(JsonOutputFormatter jsonMediaTypeFormatter, string callbackQueryParameter = null)
        {
            if (jsonMediaTypeFormatter == null)
            {
                throw new ArgumentNullException(nameof(jsonMediaTypeFormatter));
            }

            _jsonMediaTypeFormatter = jsonMediaTypeFormatter;
            _callbackQueryParameter = callbackQueryParameter ?? "callback";

            SupportedMediaTypes.Add(_textJavaScript);
            SupportedMediaTypes.Add(_applicationJavaScript);
            SupportedMediaTypes.Add(_applicationJsonp);
            foreach (var encoding in _jsonMediaTypeFormatter.SupportedEncodings)
            {
                SupportedEncodings.Add(encoding);
            }
        }

        public override bool CanWriteResult(OutputFormatterCanWriteContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return _jsonMediaTypeFormatter.CanWriteResult(context);
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            string callback;
            if (IsJsonpRequest(context.HttpContext.Request, _callbackQueryParameter, out callback))
            {
                if (!CallbackValidator.IsValid(callback))
                {
                    throw new InvalidOperationException($"Callback '{callback}' is invalid!");
                }

                using (var writer = context.WriterFactory(context.HttpContext.Response.Body, selectedEncoding))
                {
                    // the /**/ is a specific security mitigation for "Rosetta Flash JSONP abuse"
                    // the typeof check is just to reduce client error noise
                    writer.Write("/**/ typeof " + callback + " === 'function' && " + callback + "(");
                    writer.Flush();
                    _jsonMediaTypeFormatter.WriteObject(writer, context.Object);
                    writer.Write(");");
                    await writer.FlushAsync();
                }
            }
            else
            {
                await _jsonMediaTypeFormatter.WriteResponseBodyAsync(context, selectedEncoding);
            }
        }

        internal static bool IsJsonpRequest(HttpRequest request, string callbackQueryParameter, out string callback)
        {
            callback = null;

            if (request == null || request.Method.ToUpperInvariant() != "GET")
            {
                return false;
            }

            callback = request.Query
                .Where(kvp => kvp.Key.Equals(callbackQueryParameter, StringComparison.OrdinalIgnoreCase))
                .Select(kvp => kvp.Value)
                .FirstOrDefault();

            return !string.IsNullOrEmpty(callback);
        }
    }
}