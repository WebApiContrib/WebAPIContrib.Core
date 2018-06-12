using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WebApiContrib.Core.Formatter.Csv
{
	internal class FormattingConfigurationMetadataFacade<TEntity> : IFormattingConfigurationMetadataFacade<TEntity>
	{
		private readonly IFormattingConfigurationMetadata<TEntity> _metadata;

		public FormattingConfigurationMetadataFacade(IFormattingConfigurationMetadata<TEntity> metadata)
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
				var expression = item.Value.ReadMetadata as Expression<Func<TEntity, object>>;
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

		public void SetValues(object entity, string[] values)
		{
			for (int i = 0; i < values.Length; i++)
			{
				string value = values[i];
				var propertyMetadata = _metadata.PropertiesMetadata.Values.ElementAt(i);
				object convertedValue = Convert.ChangeType(value, propertyMetadata.PropertyType);
				var assignLambda = propertyMetadata.WriteMetadata as LambdaExpression;
				assignLambda.Compile().DynamicInvoke(entity, convertedValue);
			}
		}
	}
}