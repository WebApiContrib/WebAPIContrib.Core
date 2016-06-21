using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;

namespace WebApiContrib.Core.Formatter.PlainText
{
    public class PlainTextFormatterOptions
    {
        public Encoding[] SupportedEncodings { get; set; } = { Encoding.UTF8 };

        public string[] SupportedMediaTypes { get; set; } = { "text/plain" };
    }
}
