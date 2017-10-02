using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using WebApiContrib.Core.Filters;
using Microsoft.AspNetCore.Mvc.Internal;
using System.Net.Http;

namespace WebApiContrib.Core.Binding
{
    public class ValidationApplicationModelConvention : IApplicationModelConvention
    {

        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                foreach (var action in controller.Actions)
                {
                    foreach (var selector in action.Selectors)
                    {
                        if (IsGetOrHeadRequest(selector))
                        {
                            return;
                        }
                        action.Filters.Add(new ValidationAttribute());
                    }
                }
            }
        }

        private bool IsGetOrHeadRequest(SelectorModel selector)
        {
            return selector.ActionConstraints.OfType<HttpMethodActionConstraint>().Any(c => c.HttpMethods.Contains(HttpMethod.Get.Method))
                            || selector.ActionConstraints.OfType<HttpMethodActionConstraint>().Any(c => c.HttpMethods.Contains(HttpMethod.Head.Method));
        }
    }
}
