using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace WebApiContrib.Core.Formatter.PlainText
{
    public static class MvcOptionsExtensions
    {
        public static void AddPlainTextInputFormatter(this MvcOptions opts, PlainTextFormatterOptions formatterOptions)
        {
            opts.InputFormatters.Add(new PlainTextInputFormatter(formatterOptions));
        }

        public static void AddPlainTextOutputFormatter(this MvcOptions opts, PlainTextFormatterOptions formatterOptions)
        {
            opts.OutputFormatters.RemoveType<StringOutputFormatter>();
            opts.OutputFormatters.Add(new PlainTextOutputFormatter(formatterOptions));
        }

        public static void AddPlainTextFormatters(this MvcOptions opts)
        {
            var formatterOpts = new PlainTextFormatterOptions();
            opts.AddPlainTextInputFormatter(formatterOpts);
            opts.AddPlainTextOutputFormatter(formatterOpts);
        }

        public static void AddPlainTextFormatters(this MvcOptions opts, PlainTextFormatterOptions formatterOptions)
        {
            opts.AddPlainTextInputFormatter(formatterOptions);
            opts.AddPlainTextOutputFormatter(formatterOptions);
        }
    }
}