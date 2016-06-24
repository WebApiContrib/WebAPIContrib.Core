using System;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;

namespace WebApiContrib.Core.Formatter.Jsonp
{
    public static class JsonpFormatterMvcBuilderExtensions
    {
        public static IMvcBuilder AddJsonpOutputFormatter(this IMvcBuilder builder, OutputFormatter jsonFormatter, string callbackQueryParameter = null)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            return builder.AddMvcOptions(opts =>
            {
                callbackQueryParameter = callbackQueryParameter ?? "callback";
                opts.FormatterMappings.SetMediaTypeMappingForFormat(callbackQueryParameter, "text/javascript");
                opts.OutputFormatters.Add(new JsonpMediaTypeFormatter(jsonFormatter, callbackQueryParameter));
            });
        }
    }
}