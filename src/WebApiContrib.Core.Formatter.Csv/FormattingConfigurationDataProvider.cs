using System;
using System.Collections.Generic;

namespace WebApiContrib.Core.Formatter.Csv
{
    internal class FormattingConfigurationDataProvider<TEntity> : IFormattingConfigurationDataProvider<TEntity>
	{
		private readonly IFormattingConfigurationMetadata<TEntity> _metadata;

		public FormattingConfigurationDataProvider(IFormattingConfigurationMetadata<TEntity> metadata)
		{
			this._metadata = metadata;
		}

        public ICollection<string> GetHeaders()
        {
            return _metadata.PropertiesMetadata.Keys;
        }

		public IDictionary<string, string> GetFormattedValues(object entity)
		{
			Dictionary<string, string> values = new Dictionary<string, string>();
			foreach (var item in _metadata.PropertiesMetadata)
			{
                var expression = item.Value;
                // Check if entity's nested properties defined by the expression exist
                var willThrow = expression.WillThrowNullReferenceException(entity);
                var result = willThrow ? String.Empty : expression
					.Compile()
					.Invoke((TEntity)entity);
				values.Add(
					item.Key,
                    Convert.ToString(
						result,
						_metadata.FormatProvider));
			}
			return values;
		}

        public CsvFormatterOptions GetOptions()
		{
			return new CsvFormatterOptions
			{
				CsvDelimiter = _metadata.CsvDelimiter,
				Encoding = _metadata.Encoding,
				UseSingleLineHeaderInCsv = _metadata.UseHeaders
			};
		}
	}
}