﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace WebApiContrib.Core.Formatter.Csv
{
    /// <summary>
    /// ContentType: text/csv
    /// </summary>
    public class CsvInputFormatter : InputFormatter
    {
        private readonly CsvFormatterOptions _options;

        public CsvInputFormatter(CsvFormatterOptions csvFormatterOptions)
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            _options = csvFormatterOptions ?? throw new ArgumentNullException(nameof(csvFormatterOptions));
        }

        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var type = context.ModelType;
            var request = context.HttpContext.Request;
            MediaTypeHeaderValue requestContentType = null;
            MediaTypeHeaderValue.TryParse(request.ContentType, out requestContentType);

            var result = ReadStream(type, request.Body);
            return InputFormatterResult.SuccessAsync(result);
        }

        public override bool CanRead(InputFormatterContext context)
        {
            if (context.ModelType == null)
                throw new ArgumentNullException(nameof(context.ModelType));

            return IsTypeOfIEnumerable(context.ModelType);
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

        private object ReadStream(Type type, Stream stream)
        {
            Type itemType;
            var typeIsArray = false;
            IList list;
            if (type.GetGenericArguments().Length > 0)
            {
                itemType = type.GetGenericArguments()[0];
                list = (IList)Activator.CreateInstance(type);
            }
            else
            {
                typeIsArray = true;
                itemType = type.GetElementType();

                var listType = typeof(List<>);
                var constructedListType = listType.MakeGenericType(itemType);

                list = (IList)Activator.CreateInstance(constructedListType);
            }

            var reader = new StreamReader(stream, Encoding.GetEncoding(_options.Encoding));

            bool skipFirstLine = _options.UseSingleLineHeaderInCsv;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(_options.CsvDelimiter.ToCharArray());
                if(skipFirstLine)
                {
                    skipFirstLine = false;
                }
                else
                {
                    var itemTypeInGeneric = list.GetType().GetTypeInfo().GenericTypeArguments[0];
                    var item = Activator.CreateInstance(itemTypeInGeneric);
                    var properties = item.GetType().GetProperties();
                    for (int i = 0;i<values.Length; i++)
                    {
                        properties[i].SetValue(item, Convert.ChangeType(values[i], properties[i].PropertyType), null);
                    }

                    list.Add(item);
                }

            }

            if(typeIsArray)
            {
                Array array = Array.CreateInstance(itemType, list.Count);

                for(int t = 0; t < list.Count; t++)
                {
                    array.SetValue(list[t], t);
                }
                return array;
            }
            
            return list;
        }
    }
}