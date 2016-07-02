using System;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace WebApiContrib.Core.Formatter.Jsonp
{
    public static class MvcOptionsExtensions
    {
        public static void AddJsonpOutputFormatter(this MvcOptions mvcOptions, JsonOutputFormatter jsonFormatter = null, string callbackQueryParameter = null)
        {
            jsonFormatter = jsonFormatter ?? mvcOptions.OutputFormatters.OfType<JsonOutputFormatter>().FirstOrDefault();

            if (jsonFormatter == null) throw new Exception("JSON formatter must be provided or registered in MvcOptions");

            callbackQueryParameter = callbackQueryParameter ?? "callback";
            mvcOptions.FormatterMappings.SetMediaTypeMappingForFormat(callbackQueryParameter, "text/javascript");
            mvcOptions.OutputFormatters.Insert(0, new JsonpMediaTypeFormatter(jsonFormatter, callbackQueryParameter));
        }
    }
}