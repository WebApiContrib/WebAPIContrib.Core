using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace WebApiContrib.Core.Formatter.Csv
{
    internal class FluentCsvOutputFormatter : CsvOutputFormatterBase
	{
		private readonly ICollection<Type> _registredEntityTypes;

		public FluentCsvOutputFormatter(ICollection<Type> registredEntityTypes)
		{
			this._registredEntityTypes = registredEntityTypes;
		}

		protected override bool CanWriteType(Type type)
		{
			return base.CanWriteType(type) && _registredEntityTypes.Contains(type.GenericTypeArguments[0]);
		}

		protected override IEnumerable<string> GetHeaders(OutputFormatterWriteContext context)
		{
			var valuesProvider = GetDynamicDataProvider(context);
			return valuesProvider.GetHeaders() as ICollection<string>;
		}

		protected override CsvFormatterOptions GetOptions(OutputFormatterWriteContext context)
		{
			var valuesProvider = GetDynamicDataProvider(context);
			return valuesProvider.GetOptions() as CsvFormatterOptions;
		}

		protected override IEnumerable<object[]> GetValues(OutputFormatterWriteContext context)
		{
			var valuesProvider = GetDynamicDataProvider(context);
			foreach (var item in (IEnumerable<object>)context.Object)
			{
				var values = valuesProvider.GetFormattedValues(item) as IDictionary<string, string>;
				yield return values.Values.ToArray();
			}
		}

		private dynamic GetDynamicDataProvider(OutputFormatterWriteContext context)
		{
			Type type = GetItemType(context);
			var serviceProvider = context.HttpContext.RequestServices;
			Type generic = typeof(IFormattingConfigurationDataProvider<>);
			Type constructed = generic.MakeGenericType(type);
			return serviceProvider.GetService(constructed);
		}
	}
}