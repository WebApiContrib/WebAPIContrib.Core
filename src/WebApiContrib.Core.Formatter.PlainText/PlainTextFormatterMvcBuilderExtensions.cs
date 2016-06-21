using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;

namespace WebApiContrib.Core.Formatter.PlainText
{
    public static class PlainTextFormatterMvcBuilderExtensions
    {
        public static IMvcBuilder AddPlainTextFormatters(this IMvcBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            var formatterOptions = new PlainTextFormatterOptions();
            return builder.AddPlainTextFormatters(formatterOptions);
        }

        public static IMvcBuilder AddPlainTextFormatters(this IMvcBuilder builder, PlainTextFormatterOptions formatterOptions)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            return builder.AddPlainTextInputFormatter(formatterOptions).AddPlainTextOutputFormatter(formatterOptions);
        }

        public static IMvcBuilder AddPlainTextInputFormatter(this IMvcBuilder builder, PlainTextFormatterOptions formatterOptions)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            return builder.AddMvcOptions(opts => opts.InputFormatters.Add(new PlainTextInputFormatter(formatterOptions)));
        }

        public static IMvcBuilder AddPlainTextOutputFormatter(this IMvcBuilder builder, PlainTextFormatterOptions formatterOptions)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            return builder.AddMvcOptions(opts =>
            {
                opts.OutputFormatters.RemoveType<StringOutputFormatter>();
                opts.OutputFormatters.Add(new PlainTextOutputFormatter(formatterOptions));
            });
        }
    }
}