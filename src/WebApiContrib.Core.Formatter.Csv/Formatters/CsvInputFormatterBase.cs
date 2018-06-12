using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace WebApiContrib.Core.Formatter.Csv
{
    public abstract class CsvInputFormatterBase : InputFormatter
	{
		public CsvInputFormatterBase()
		{
			SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
		}

		public override bool CanRead(InputFormatterContext context)
		{
			if (context.ModelType == null)
				throw new ArgumentNullException(nameof(context.ModelType));
			return IsTypeOfIEnumerable(GetType(context));
		}

		protected Type GetType(InputFormatterContext context)
		{
			return context.ModelType;
		}

		protected Type GetItemType(InputFormatterContext context)
		{
			Type type = GetType(context);
			return IsTypeOfGenericList(type) ? type.GetGenericArguments()[0] : type.GetElementType();
		}

		private bool IsTypeOfIEnumerable(Type type)
		{
			foreach (Type interfaceType in type.GetInterfaces())
			{
				if (interfaceType == typeof(IList))
					return true;
			}
			return false;
		}

		protected bool IsTypeOfGenericList(Type type)
		{
			return type.GetGenericArguments().Length > 0;
		}

		protected abstract CsvFormatterOptions GetOptions(InputFormatterContext context);

        protected abstract void SetValues(InputFormatterContext context, object entity, string[] values);

		public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
		{
			var request = context.HttpContext.Request;
			var serviceProvider = context.HttpContext.RequestServices;
			var options = GetOptions(context);
			var type = GetType(context);
			var itemType = GetItemType(context);
			var genericListType = typeof(List<>).MakeGenericType(itemType);
			var list = (IList)Activator.CreateInstance(genericListType);
			var reader = new StreamReader(request.Body, Encoding.GetEncoding(options.Encoding));
			bool skipFirstLine = options.UseSingleLineHeaderInCsv;
            
			while (!reader.EndOfStream)
			{
				var line = reader.ReadLine();
				var values = line.Split(options.CsvDelimiter.ToCharArray());

				if (skipFirstLine)
				{
					skipFirstLine = false;
				}
				else
				{
					var item = Activator.CreateInstance(itemType);
                    SetValues(context, item, values);
					list.Add(item);
				}
			}

			if (!IsTypeOfGenericList(type))
			{
				Array array = Array.CreateInstance(itemType, list.Count);
				list.CopyTo(array, 0);
				return InputFormatterResult.SuccessAsync(array);
			}

			return InputFormatterResult.SuccessAsync(list);
		}
	}
}