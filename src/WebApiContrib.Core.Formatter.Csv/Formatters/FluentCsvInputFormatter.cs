using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace WebApiContrib.Core.Formatter.Csv
{
    public class FluentCsvInputFormatter : CsvInputFormatterBase
	{
		private readonly ICollection<Type> _registredEntityTypes;

		public FluentCsvInputFormatter(ICollection<Type> registredEntityTypes)
		{
			this._registredEntityTypes = registredEntityTypes;
		}

        public override bool CanRead(InputFormatterContext context)
		{
            var type = GetType(context);
			return base.CanReadType(type) && _registredEntityTypes.Contains(type.GenericTypeArguments[0]);
		}

		protected override CsvFormatterOptions GetOptions(InputFormatterContext context)
		{
			var metadataFacade = GetDynamicConfigurationMetadataFacade(context);
			return metadataFacade.GetOptions() as CsvFormatterOptions;
		}

		protected override void SetValues(InputFormatterContext context, object entity, string[] values)
		{
            var metadataFacade = GetDynamicConfigurationMetadataFacade(context);
			metadataFacade.SetValues(entity, values);
		}

		private dynamic GetDynamicConfigurationMetadataFacade(InputFormatterContext context)
		{
			Type type = GetItemType(context);
			var serviceProvider = context.HttpContext.RequestServices;
			Type generic = typeof(IFormattingConfigurationMetadataFacade<>);
			Type constructed = generic.MakeGenericType(type);
			return serviceProvider.GetService(constructed);
		}
	}
}