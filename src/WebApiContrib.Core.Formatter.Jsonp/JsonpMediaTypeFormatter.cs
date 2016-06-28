using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace WebApiContrib.Core.Formatter.Jsonp
{
    public class JsonpMediaTypeFormatter : OutputFormatter
    {
        private static readonly MediaTypeHeaderValue _applicationJavaScript = new MediaTypeHeaderValue("application/javascript");
        private static readonly MediaTypeHeaderValue _applicationJsonp = new MediaTypeHeaderValue("application/json-p");
        private static readonly MediaTypeHeaderValue _textJavaScript = new MediaTypeHeaderValue("text/javascript");
        private readonly OutputFormatter _jsonMediaTypeFormatter;
        private readonly string _callbackQueryParameter;

        public JsonpMediaTypeFormatter(OutputFormatter jsonMediaTypeFormatter, string callbackQueryParameter = null)
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
            //foreach (var encoding in _jsonMediaTypeFormatter.SupportedMediaTypes)
            //{
            //    SupportedEncodings.Add(encoding);
            //}

            //MediaTypeMappings.Add(new JsonpQueryStringMapping(_callbackQueryParameter, _textJavaScript));
        }

        public override bool CanWriteResult(OutputFormatterCanWriteContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return _jsonMediaTypeFormatter.CanWriteResult(context);
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
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

                using (var writer = new StreamWriter(context.HttpContext.Response.Body, Encoding.UTF8, bufferSize: 4096, leaveOpen: true))
                {
                    // the /**/ is a specific security mitigation for "Rosetta Flash JSONP abuse"
                    // the typeof check is just to reduce client error noise
                    writer.Write("/**/ typeof " + callback + " === 'function' && " + callback + "(");
                    writer.Flush();
                    await _jsonMediaTypeFormatter.WriteResponseBodyAsync(context);
                    writer.Write(");");
                    writer.Flush();
                }
            }
            else
            {
                throw new InvalidOperationException("Not a valid JSONP request.");
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