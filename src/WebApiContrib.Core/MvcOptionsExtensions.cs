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

        /// <summary>
        /// Registers a Validation action filter as application model convention, which applies this filter to POST, PUT, and DELETE actions.
        /// </summary>
        /// <param name="controllerPredicate">A controller predicate for more granular application of the filter.</param>
        /// <param name="actionPredicate">An action predicate for more granular application of the filter.</param>
        public static void UseValidationFilter(this MvcOptions opts, Func<ControllerModel, bool> controllerPredicate = null, Func<ActionModel, bool> actionPredicate = null)
    => UseValidationFilter(opts, new ValidationAttribute(), controllerPredicate, actionPredicate);

        /// <summary>
        /// Registers a Validation action filter as application model convention, which applies this filter to POST, PUT, and DELETE actions.
        /// </summary>
        /// <param name="filter">A filter of type ValidationAttribute.</param>
        /// <param name="controllerPredicate">A controller predicate for more granular application of the filter.</param>
        /// <param name="actionPredicate">An action predicate for more granular application of the filter.</param>
        public static void UseValidationFilter(this MvcOptions opts, ValidationAttribute filter, Func<ControllerModel, bool> controllerPredicate = null, Func<ActionModel, bool> actionPredicate = null)
        {
            // Exclude GET and HEAD actions
            Func<SelectorModel, bool> selectorPredicate = s =>
            s.ActionConstraints.OfType<HttpMethodActionConstraint>().Any(c => !c.HttpMethods.Contains(HttpMethod.Get.Method)
     || !c.HttpMethods.Contains(HttpMethod.Head.Method));

            AddGlobalFilter(opts, filter, controllerPredicate, actionPredicate, selectorPredicate);
        }

        /// <summary>
        /// Registers a filter as global application model convention.
        /// </summary>
        /// <param name="filter">A custom filter to register.</param>
        /// <param name="controllerPredicate">A controller predicate for more granular application of the filter.</param>
        /// <param name="actionPredicate">An action predicate for more granular application of the filter.</param>
        /// <param name="selectorPredicate">A selector predicate for more granular application of the filter.</param>
        public static void AddGlobalFilter(this MvcOptions opts, IFilterMetadata filter, Func<ControllerModel, bool> controllerPredicate = null, Func<ActionModel, bool> actionPredicate = null,
            Func<SelectorModel, bool> selectorPredicate = null)
        {
            opts.Conventions.Insert(0, new GlobalFilterApplicationModelConvention(controllerPredicate, actionPredicate, selectorPredicate, filter));
        }
    }
}
