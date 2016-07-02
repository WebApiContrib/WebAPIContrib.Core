using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace WebApiContrib.Core.Samples.Controllers
{
    [Route("api/values")]
    public class ValuesController : Controller
    {
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }
    }
}