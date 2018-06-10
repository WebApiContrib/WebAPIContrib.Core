namespace WebApiContrib.Core.Formatter.Csv
{
	internal interface IFormattingConfigurationMetadataProvider<TEntity>
	{
		IFormattingConfigurationMetadata<TEntity> Metadata { get; }
	}
}