using System;
using System.Linq.Expressions;

namespace WebApiContrib.Core.Formatter.Csv
{
	public class PropertyAccessMetadata
	{
		public Expression ReadMetadata { get; set; }
		public Expression WriteMetadata { get; set; }
		public Type PropertyType { get; set; }
	}
}