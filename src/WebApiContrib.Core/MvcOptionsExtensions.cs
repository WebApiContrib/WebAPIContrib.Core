using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using WebApiContrib.Core.Routing;

namespace WebApiContrib.Core
{
    public static class MvcOptionsExtensions
    {
        public static void UseGlobalRoutePrefix(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
        {
            opts.Conventions.Insert(0, new GlobalRoutePrefixConvention(routeAttribute));
        }
    }
}
