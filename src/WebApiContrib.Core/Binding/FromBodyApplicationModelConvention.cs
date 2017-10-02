using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApiContrib.Core.Internal;
using WebApiContrib.Core.Filters;
using Microsoft.AspNetCore.Mvc.Internal;
using System.Net.Http;

namespace WebApiContrib.Core.Binding
{
    public class FromBodyApplicationModelConvention : IApplicationModelConvention
    {
        private readonly Func<ControllerModel, bool> _controllerPredicate;
        private readonly Func<ActionModel, bool> _actionPredicate;
        private readonly Func<ParameterModel, bool> _parameterPredicate;

        public FromBodyApplicationModelConvention() : this(null, null, null)
        {
        }

        public FromBodyApplicationModelConvention(Func<ControllerModel, bool> controllerPredicate,
            Func<ActionModel, bool> actionPredicate, Func<ParameterModel, bool> parameterPredicate)
        {
            _controllerPredicate = controllerPredicate ?? (c => true);
            _actionPredicate = actionPredicate ?? (a => true);
            _parameterPredicate = parameterPredicate ?? (p => true);
        }

        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers.Where(_controllerPredicate))
            {
                foreach (var action in controller.Actions.Where(_actionPredicate))
                {
                    foreach (var parameter in action.Parameters.Where(parameter => parameter.BindingInfo?.BindingSource == null &&
                            !parameter.Attributes.OfType<IBindingSourceMetadata>().Any() &&
                            !parameter.ParameterInfo.ParameterType.CanBeConvertedFromString()).Where(_parameterPredicate))
                    {
                        parameter.BindingInfo = parameter.BindingInfo ?? new BindingInfo();
                        parameter.BindingInfo.BindingSource = BindingSource.Body;
                    }
                }
            }
        }




    }
}
