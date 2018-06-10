using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace WebApiContrib.Core.Formatter.Csv
{
    public class StandardCsvOutputFormatter : CsvOutputFormatterBase
	{
		private readonly CsvFormatterOptions _options;

		public StandardCsvOutputFormatter(CsvFormatterOptions csvFormatterOptions)
		{
			_options = csvFormatterOptions ?? throw new ArgumentNullException(nameof(csvFormatterOptions));
		}

		protected override IEnumerable<string> GetHeaders(OutputFormatterWriteContext context)
		{
			return base
				.GetItemType(context)
				.GetProperties()
				.Select(x => x.GetCustomAttribute<DisplayAttribute>(false)?.Name ?? x.Name);
		}

		protected override CsvFormatterOptions GetOptions(OutputFormatterWriteContext context)
		{
			return _options;
		}

		protected override IEnumerable<object[]> GetValues(OutputFormatterWriteContext context)
		{
			foreach (var item in (IEnumerable<object>)context.Object)
			{
				yield return item
					.GetType()
					.GetProperties()
					.Select(pi => pi.GetValue(item, null))
					.ToArray();
			}
		}
	}
}