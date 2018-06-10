using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace WebApiContrib.Core.Formatter.Csv
{
    public abstract class CsvOutputFormatterBase : OutputFormatter
	{
		public string ContentType { get; private set; }

		protected CsvOutputFormatterBase()
		{
			ContentType = "text/csv";
			SupportedMediaTypes.Add(Microsoft.Net.Http.Headers.MediaTypeHeaderValue.Parse("text/csv"));
		}

		protected override bool CanWriteType(Type type)
		{
			if (type == null)
				throw new ArgumentNullException(nameof(type));
			return IsTypeOfIEnumerable(type);
		}

		protected bool IsTypeOfIEnumerable(Type type)
		{
			foreach (Type interfaceType in type.GetInterfaces())
			{
				if (interfaceType == typeof(IList))
					return true;
			}
			return false;
		}

		protected Type GetItemType(OutputFormatterWriteContext context)
		{
			Type type = context.Object.GetType();
			return type.GetGenericArguments().Length > 0 ? type.GetGenericArguments()[0] : type.GetElementType();
		}

		protected abstract IEnumerable<string> GetHeaders(OutputFormatterWriteContext context);

		protected abstract CsvFormatterOptions GetOptions(OutputFormatterWriteContext context);

		protected abstract IEnumerable<object[]> GetValues(OutputFormatterWriteContext context);

		public async override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
		{
			var serviceProvider = context.HttpContext.RequestServices;
			var response = context.HttpContext.Response;
			var options = GetOptions(context);
			var streamWriter = new StreamWriter(response.Body, Encoding.GetEncoding(options.Encoding));

			if (options.UseSingleLineHeaderInCsv)
			{
				await streamWriter.WriteLineAsync(
					string.Join(
						options.CsvDelimiter, GetHeaders(context)
					)
				);
			}

			foreach (var values in GetValues(context))
			{
				string valueLine = string.Empty;

				foreach (var value in values)
				{
					if (value != null)
					{
						var _val = value.ToString();

						//Check if the value contans a comma and place it in quotes if so
						if (_val.Contains(","))
							_val = string.Concat("\"", _val, "\"");
						//Replace any \r or \n special characters from a new line with a space
						if (_val.Contains("\r"))
							_val = _val.Replace("\r", " ");
						if (_val.Contains("\n"))
							_val = _val.Replace("\n", " ");

						valueLine = string.Concat(valueLine, _val, options.CsvDelimiter);
					}
					else
					{
						valueLine = string.Concat(valueLine, string.Empty, options.CsvDelimiter);
					}
				}

				await streamWriter.WriteLineAsync(valueLine.TrimEnd(options.CsvDelimiter.ToCharArray()));
			}

			await streamWriter.FlushAsync();
		}
	}
}