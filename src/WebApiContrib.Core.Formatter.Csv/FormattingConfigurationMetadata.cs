using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WebApiContrib.Core.Formatter.Csv
{
    internal class FormattingConfigurationMetadata<TEntity> 
		: IFormattingConfigurationMetadata<TEntity>
	{
		public bool UseHeaders { get; set; }
		public string CsvDelimiter { get; set; } = ";";
		public string Encoding { get; set; } = "ISO-8859-1";
		public IFormatProvider FormatProvider { get; set; }
		public Dictionary<string, Expression<Func<TEntity, object>>> PropertiesMetadata { get; } = new Dictionary<string, Expression<Func<TEntity, object>>>();
	}
}