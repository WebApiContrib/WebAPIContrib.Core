using System;

namespace WebApiContrib.Core.Formatter.Csv
{
    public interface IFormattingConfigurationBuilder<TEntity>
	{
		IFormattingConfigurationPropertyBuilder<TEntity> ForHeader();
		IFormattingConfigurationPropertyBuilder<TEntity> ForHeader(string name);
		IFormattingConfigurationBuilder<TEntity> UseCsvDelimiter(string delimiter);
		IFormattingConfigurationBuilder<TEntity> UseEncoding(string encoding);
		IFormattingConfigurationBuilder<TEntity> UseFormatProvider(IFormatProvider formatProvider);
		IFormattingConfigurationBuilder<TEntity> UseHeaders();
	}
}