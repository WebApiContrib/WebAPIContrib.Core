using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using WebApiContrib.Core.Razor.TagHelper;

namespace WebApiContrib.Core.Razor
{
    public static class ServiceCollectionExtensions
    {
        public static void EnableAddTagHelperAssemblyGlobbing(this IServiceCollection services)
        {
            services.AddSingleton<ITagHelperTypeResolver, AssemblyNameGlobbingTagHelperTypeResolver>();
        }
    }
}