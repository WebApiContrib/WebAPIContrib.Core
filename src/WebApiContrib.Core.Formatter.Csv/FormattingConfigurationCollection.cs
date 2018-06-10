using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace WebApiContrib.Core.Formatter.Csv
{
	public class FormattingConfigurationCollection : IFormattingConfigurationCollection
	{
		private readonly ICollection<Type> _registredTypes = new List<Type>();
		private readonly IServiceCollection _serviceCollection;

		public FormattingConfigurationCollection(IServiceCollection serviceCollection)
		{
			this._serviceCollection = serviceCollection;
		}

		public ICollection<Type> GetRegistredTypes()
		{
			return _registredTypes;
		}

		public void RegisterConfiguration<TEntity>(IFormattingConfiguration<TEntity> config)
		{
			var metadata = new FormattingConfigurationMetadata<TEntity>();
			var builder = new FormattingConfigurationBuilder<TEntity>(metadata);
			config.Configure(builder);
			var provider = new FormattingConfigurationDataProvider<TEntity>(metadata);
			_serviceCollection.AddSingleton<IFormattingConfigurationDataProvider<TEntity>>(provider);
			_registredTypes.Add(typeof(TEntity));
		}
	}
}