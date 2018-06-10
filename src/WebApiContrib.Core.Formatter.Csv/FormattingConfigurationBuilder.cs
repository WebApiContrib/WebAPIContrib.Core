using System;
using System.Linq.Expressions;

namespace WebApiContrib.Core.Formatter.Csv
{
    internal class FormattingConfigurationBuilder<TEntity> :
		IFormattingConfigurationBuilder<TEntity>,
		IFormattingConfigurationPropertyBuilder<TEntity>,
		IFormattingConfigurationMetadataProvider<TEntity>
	{
		private string _lastHeader;
		private IFormattingConfigurationMetadata<TEntity> _metadata;

		public FormattingConfigurationBuilder(
			IFormattingConfigurationMetadata<TEntity> metadata)
		{
			_metadata = metadata;
		}

		public IFormattingConfigurationMetadata<TEntity> Metadata => _metadata;

		public IFormattingConfigurationPropertyBuilder<TEntity> ForHeader()
		{
			_lastHeader = null;
			return this;
		}

		public IFormattingConfigurationPropertyBuilder<TEntity> ForHeader(string name)
		{
			_lastHeader = name;
			return this;
		}

		public IFormattingConfigurationBuilder<TEntity> UseCsvDelimiter(string delimiter)
		{
			_metadata.CsvDelimiter = delimiter;
			return this;
		}

		public IFormattingConfigurationBuilder<TEntity> UseEncoding(string encoding)
		{
			_metadata.Encoding = encoding;
			return this;
		}

		public IFormattingConfigurationBuilder<TEntity> UseFormatProvider(IFormatProvider formatProvider)
		{
			_metadata.FormatProvider = formatProvider;
			return this;
		}

		public IFormattingConfigurationBuilder<TEntity> UseHeaders()
		{
			_metadata.UseHeaders = true;
			return this;
		}

        public IFormattingConfigurationBuilder<TEntity> UseProperty(Expression<Func<TEntity, object>> propertyExpression)
		{
            var memberExpression = propertyExpression.GetMemberExpression();
            if(memberExpression is null)
                throw new ArgumentException("Provided expression can only return primitive property of an entity and should not contain method calls.", nameof(propertyExpression));
            _metadata.PropertiesMetadata.Add(
                _lastHeader ?? memberExpression.GetPropertyPath(), 
                propertyExpression);
            _lastHeader = null;
            return this;
		}
	}
}