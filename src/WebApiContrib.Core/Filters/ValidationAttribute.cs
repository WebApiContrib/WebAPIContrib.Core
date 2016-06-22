using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiContrib.Core.Filters
{
    public class ValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .Select(e => new
                    {
                        Name = e.Key,
                        Message = e.Value.Errors.First().ErrorMessage
                    }).ToArray();

                actionContext.Result = new BadRequestObjectResult(errors);
            }
        }
    }
}