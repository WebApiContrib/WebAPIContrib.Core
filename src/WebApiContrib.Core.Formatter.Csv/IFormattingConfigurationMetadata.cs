using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WebApiContrib.Core.Formatter.Csv
{
	public interface IFormattingConfigurationMetadata<TEntity>
	{
		bool UseHeaders { get; set; }
		string CsvDelimiter { get; set; }
		string Encoding { get; set; }
		IFormatProvider FormatProvider { get; set; }
		IDictionary<string, PropertyAccessMetadata> PropertiesMetadata { get; }
	}
}