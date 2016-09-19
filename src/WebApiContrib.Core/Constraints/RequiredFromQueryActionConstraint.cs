using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace WebApiContrib.Core.Constraints
{
    public class RequiredFromQueryActionConstraint : IActionConstraint
    {
        private readonly string _parameter;

        public RequiredFromQueryActionConstraint(string parameter)
        {
            _parameter = parameter;
        }

        public int Order => 999;

        public bool Accept(ActionConstraintContext context)
        {
            for (var i = 0; i < _parameter.Length; i++)
            {
                if (!context.RouteContext.HttpContext.Request.Query.ContainsKey(_parameter))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
