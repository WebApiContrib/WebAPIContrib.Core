namespace WebApiContrib.Core.Formatter.Csv
{
	public interface IFormattingConfiguration<TEntity>
	{
		void Configure(IFormattingConfigurationBuilder<TEntity> builder);
	}
}