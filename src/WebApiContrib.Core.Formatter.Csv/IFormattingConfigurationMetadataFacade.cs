using System.Collections.Generic;

namespace WebApiContrib.Core.Formatter.Csv
{
	internal interface IFormattingConfigurationMetadataFacade<TEntity>
	{
        ICollection<string> GetHeaders();
		IDictionary<string, string> GetFormattedValues(object entity);
		CsvFormatterOptions GetOptions();
		void SetValues(object entity, string[] values);
	}
}