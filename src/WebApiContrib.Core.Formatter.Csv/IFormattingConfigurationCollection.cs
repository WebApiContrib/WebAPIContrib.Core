using System;
using System.Collections.Generic;

namespace WebApiContrib.Core.Formatter.Csv
{
	public interface IFormattingConfigurationCollection
	{
		ICollection<Type> GetRegistredTypes();
		void RegisterConfiguration<TEntity>(IFormattingConfiguration<TEntity> config);
	}
}