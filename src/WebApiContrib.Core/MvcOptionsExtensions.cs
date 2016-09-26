using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using WebApiContrib.Core.Binding;
using WebApiContrib.Core.Routing;

namespace WebApiContrib.Core
{
    public static class MvcOptionsExtensions
    {
        public static void UseGlobalRoutePrefix(this MvcOptions opts, string routeTemplate)
        {
            opts.Conventions.Insert(0, new GlobalRoutePrefixConvention(new RouteAttribute(routeTemplate)));
        }

        public static void UseGlobalRoutePrefix(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
        {
            opts.Conventions.Insert(0, new GlobalRoutePrefixConvention(routeAttribute));
        }

        public static void UseFromBodyBinding(this MvcOptions opts, Func<ControllerModel, bool> controllerPredicate = null,
            Func<ActionModel, bool> actionPredicate = null, Func<ParameterModel, bool> parameterPredicate = null)
        {
            opts.Conventions.Insert(0, new FromBodyApplicationModelConvention(controllerPredicate, actionPredicate, parameterPredicate));
        }
    }
}
