﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using WebApiContrib.Core.Filters;
using Microsoft.AspNetCore.Mvc.Internal;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiContrib.Core.Binding
{
    public class GlobalActionFilterApplicationModelConvention : IApplicationModelConvention
    {
        private readonly ActionFilterAttribute _filter;
        private readonly Func<ControllerModel, bool> _controllerPredicate;
        private readonly Func<ActionModel, bool> _actionPredicate;
        private readonly Func<SelectorModel, bool> _selectorPredicate;

        public GlobalActionFilterApplicationModelConvention(
            Func<ControllerModel, bool> controllerPredicate, Func<ActionModel, bool> actionPredicate, Func<SelectorModel, bool> selectorPredicate, ActionFilterAttribute filter)
        {
            _controllerPredicate = controllerPredicate ?? (c => true);
            _actionPredicate = actionPredicate ?? (a => true);
            _selectorPredicate = selectorPredicate ?? (s => true);
            _filter = filter;
        }

        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers.Where(_controllerPredicate))
            {
                foreach (var action in controller.Actions.Where(_actionPredicate))
                {
                    foreach (var selector in action.Selectors.Where(_selectorPredicate))
                    {
                        action.Filters.Add(_filter);
                    }
                }
            }
        }
    }
}