using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace WebApiContrib.Core.Formatter.Bson
{
    public static class MvcBsonMvcBuilderExtensions
    {
        public static IMvcBuilder AddBsonSerializerFormatters(this IMvcBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, MvcBsonSerializerOptionsSetup>());
            return builder;
        }
    }
}
