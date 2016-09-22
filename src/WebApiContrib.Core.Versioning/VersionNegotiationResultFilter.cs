using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace WebApiContrib.Core.Versioning
{
    /// <summary>
    /// A filter to negotiate resource version representations based on different version strategies.
    /// </summary>
    public class VersionNegotiationResultFilter : IResultFilter
    {
        /// <summary>
        /// Creates a new instance of <see cref="VersionNegotiationResultFilter"/>.
        /// </summary>
        /// <param name="serviceProvider">The service provider to get strategies and mappers from.</param>
        /// <param name="options">The versioning options.</param>
        public VersionNegotiationResultFilter(IServiceProvider serviceProvider, IOptions<VersionNegotiationOptions> options)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            ServiceProvider = serviceProvider;
            Options = options;
        }

        private IServiceProvider ServiceProvider { get; }

        private IOptions<VersionNegotiationOptions> Options { get; }

        /// <inheritdoc />
        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var result = Options.Value.RequireVersionedObjectResult
                ? context.Result as VersionedObjectResult
                : context.Result as ObjectResult;

            if (result == null)
            {
                return;
            }

            var strategy = ServiceProvider.GetRequiredService<IVersionStrategy>();

            var version = strategy.GetVersion(context.HttpContext, context.RouteData);

            context.Result = MapResult(result, version);
        }

        /// <inheritdoc />
        public void OnResultExecuted(ResultExecutedContext context)
        {
            // Meh. Not used.
        }

        private IActionResult MapResult(ObjectResult result, int? version)
        {
            Type itemType;
            if (TryGetCollectionType(result.Value, out itemType))
            {
                return MapCollectionResult(result, version, itemType);
            }

            return MapSingleResult(result, version);
        }

        private IActionResult MapCollectionResult(ObjectResult result, int? version, Type itemType)
        {
            var mapperType = typeof(IModelMapper<>).MakeGenericType(itemType);

            var mapperTypeInfo = mapperType.GetTypeInfo();

            var method = mapperTypeInfo.GetMethod(nameof(IModelMapper<string>.Map));

            object mapper;

            try
            {
                mapper = ServiceProvider.GetRequiredService(mapperType);
            }
            catch (Exception ex)
            {
                if (Options.Value.ThrowOnMissingMapper)
                {
                    throw new MissingModelMapperException(itemType, ex);
                }

                return result;
            }

            var list = new List<object>();

            var collection = (IEnumerable<object>) result.Value;

            foreach (var item in collection)
            {
                list.Add(method.Invoke(mapper, new[] { item, version }));
            }

            return new ObjectResult(list);
        }

        private IActionResult MapSingleResult(ObjectResult result, int? version)
        {
            var value = result.Value;

            var valueType = value.GetType();

            var mapperType = typeof(IModelMapper<>).MakeGenericType(valueType);

            var mapperTypeInfo = mapperType.GetTypeInfo();

            var method = mapperTypeInfo.GetMethod(nameof(IModelMapper<string>.Map));

            object mapper;

            try
            {
                mapper = ServiceProvider.GetRequiredService(mapperType);
            }
            catch (Exception ex)
            {
                if (Options.Value.ThrowOnMissingMapper)
                {
                    throw new MissingModelMapperException(valueType, ex);
                }

                return result;
            }

            var newValue = method.Invoke(mapper, new[] { value, version });

            return new ObjectResult(newValue);
        }

        private static bool TryGetCollectionType(object value, out Type itemType)
        {
            var type = value.GetType();

            if (type != typeof(string))
            {
                if (type.IsArray)
                {
                    itemType = type.GetElementType();
                    return true;
                }

                Type[] typeArguments;
                if (type.IsAssignableToGenericTypeDefinition(typeof(IEnumerable<>), out typeArguments))
                {
                    itemType = typeArguments[0];
                    return true;
                }
            }

            itemType = default(Type);
            return false;
        }
    }
}