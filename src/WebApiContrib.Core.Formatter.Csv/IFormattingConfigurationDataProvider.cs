using System.Collections.Generic;

namespace WebApiContrib.Core.Formatter.Csv
{
	internal interface IFormattingConfigurationDataProvider<TEntity>
	{
        ICollection<string> GetHeaders();
		IDictionary<string, string> GetFormattedValues(object entity);
		CsvFormatterOptions GetOptions();
	}
}