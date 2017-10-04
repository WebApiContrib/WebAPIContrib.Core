using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using WebApiContrib.Core.Binding;
using WebApiContrib.Core.Routing;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Internal;
using System.Net.Http;
using WebApiContrib.Core.Filters;

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

        public static void UseValidationFilter(this MvcOptions opts, ActionFilterAttribute filter = null, Func<ControllerModel, bool> controllerPredicate = null, Func<ActionModel, bool> actionPredicate = null,
            Func<SelectorModel, bool> selectorPredicate = null)
        {
            selectorPredicate = s =>
            {
                return s.ActionConstraints.OfType<HttpMethodActionConstraint>().Any(c => !c.HttpMethods.Contains(HttpMethod.Get.Method)
                || !c.HttpMethods.Contains(HttpMethod.Head.Method));
            };

            if (filter == null)
            {
                filter = new ValidationAttribute();
            }

            AddGlobalFilter(opts, filter, controllerPredicate, actionPredicate, selectorPredicate);
        }

        public static void AddGlobalFilter(this MvcOptions opts, ActionFilterAttribute filter, Func<ControllerModel, bool> controllerPredicate = null, Func<ActionModel, bool> actionPredicate = null, 
            Func<SelectorModel, bool> selectorPredicate = null)
        {
            opts.Conventions.Insert(0, new GlobalActionFilterApplicationModelConvention(controllerPredicate, actionPredicate, selectorPredicate, filter));
        }
    }
}
