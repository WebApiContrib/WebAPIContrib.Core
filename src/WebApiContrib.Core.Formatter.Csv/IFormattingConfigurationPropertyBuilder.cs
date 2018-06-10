using System;
using System.Linq.Expressions;

namespace WebApiContrib.Core.Formatter.Csv
{
    public interface IFormattingConfigurationPropertyBuilder<TEntity>
	{
		IFormattingConfigurationBuilder<TEntity> UseProperty(Expression<Func<TEntity, object>> source);
	}
}