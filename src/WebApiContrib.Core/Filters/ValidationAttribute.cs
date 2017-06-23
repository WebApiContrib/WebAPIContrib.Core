using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;

namespace WebApiContrib.Core.Filters
{
    public class ValidationAttribute : ActionFilterAttribute
    {
        public bool AllowNull { get; set; }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (!AllowNull)
            {
                var nullArguments = actionContext.ActionArguments
                    .Where(arg => arg.Value == null)
                    .Select(arg => new Error
                    {
                        Name = arg.Key,
                        Message = "Value cannot be null."
                    }).ToArray();

                if (nullArguments.Any())
                {
                    actionContext.Result = new BadRequestObjectResult(nullArguments);
                    return;
                }
            }

            if (!actionContext.ModelState.IsValid)
            {
                var errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .Select(e => new Error
                    {
                        Name = e.Key,
                        Message = e.Value.Errors.First().ErrorMessage
                    }).ToArray();

                actionContext.Result = new BadRequestObjectResult(errors);
            }
        }

        private class Error
        {
            public string Name { get; set; }

            public string Message { get; set; }
        }
    }
}
