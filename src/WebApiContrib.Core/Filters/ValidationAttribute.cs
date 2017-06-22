using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;

namespace WebApiContrib.Core.Filters
{
    public class ValidationAttribute : ActionFilterAttribute
    {
        public bool AllowNull { get; }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (!AllowNull)
            {
                var nullArguments = actionContext.ActionArguments.Where(x => x.Value == null);
                if (nullArguments.Any())
                {
                    var errors = new List<Error>();
                    foreach(var nullArgument in nullArguments)
                    {
                        errors.Add(new Error
                        {
                            Name = nullArgument.Key,
                            Message = "Value cannot be null."
                        });
                    }
                    actionContext.Result = new BadRequestObjectResult(errors);
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