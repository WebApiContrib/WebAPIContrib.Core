using System;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace WebApiContrib.Core.Formatter.Csv
{
    public class StandardCsvInputFormatter : CsvInputFormatterBase
    {
        private readonly CsvFormatterOptions _options;

        public StandardCsvInputFormatter(CsvFormatterOptions context)
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            _options = context ?? throw new ArgumentNullException(nameof(context));
        }

		protected override CsvFormatterOptions GetOptions(InputFormatterContext context)
		{
			return _options;
		}

		protected override void SetValues(InputFormatterContext context, object entity, string[] values)
		{
            var properties = entity.GetType().GetProperties();
            for (int i = 0; i < values.Length; i++)
            {
            	properties[i].SetValue(entity, Convert.ChangeType(values[i], properties[i].PropertyType), null);
            }
		}
	}
}