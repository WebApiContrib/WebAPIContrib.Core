using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WebApiContrib.Core.Formatter.Csv
{
	internal interface IFormattingConfigurationMetadata<TEntity>
	{
		bool UseHeaders { get; set; }
		string CsvDelimiter { get; set; }
		string Encoding { get; set; }
		IFormatProvider FormatProvider { get; set; }
		Dictionary<string, Expression<Func<TEntity, object>>> PropertiesMetadata { get; }
	}
}