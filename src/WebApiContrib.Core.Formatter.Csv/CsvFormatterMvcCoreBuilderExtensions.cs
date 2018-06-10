﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace WebApiContrib.Core.Formatter.Csv
{
    public static class CsvFormatterMvcCoreBuilderExtensions
    {
        public static IMvcCoreBuilder AddCsvSerializerFormatters(this IMvcCoreBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return AddCsvSerializerFormatters(builder, csvFormatterOptions: null);
        }

        public static IMvcCoreBuilder AddCsvSerializerFormatters(this IMvcCoreBuilder builder, CsvFormatterOptions csvFormatterOptions)
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

            if (string.IsNullOrWhiteSpace(csvFormatterOptions.CsvDelimiter))
            {
                throw new ArgumentException("CsvDelimiter cannot be empty");
            }

            builder.AddMvcOptions(options => options.InputFormatters.Add(new CsvInputFormatter(csvFormatterOptions)));
            builder.AddMvcOptions(options => options.OutputFormatters.Add(new StandardCsvOutputFormatter(csvFormatterOptions)));

            return builder;
        }
    }
}