using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Net.Http.Headers;

namespace WebApiContrib.Core.Formatter.Csv
{
    public static class CsvFormatterMvcBuilderExtensions
    {
        public static IMvcBuilder AddCsvSerializerFormatters(this IMvcBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return AddCsvSerializerFormatters(builder, csvFormatterOptions: null);
        }

        public static IMvcBuilder AddCsvSerializerFormatters( this IMvcBuilder builder, CsvFormatterOptions csvFormatterOptions)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddFormatterMappings(m => m.SetMediaTypeMappingForFormat("csv", new MediaTypeHeaderValue("text/csv")));

            if (csvFormatterOptions == null)
            {
                csvFormatterOptions = new CsvFormatterOptions();
            }

            if (string.IsNullOrEmpty(csvFormatterOptions.CsvDelimiter))
            {
                throw new ArgumentException("CsvDelimiter cannot be empty");
            }

            builder.AddMvcOptions(options => options.InputFormatters.Add(new CsvInputFormatter(csvFormatterOptions)));
            builder.AddMvcOptions(options => options.OutputFormatters.Add(new CsvOutputFormatter(csvFormatterOptions)));


            return builder;
        }
    }
}
