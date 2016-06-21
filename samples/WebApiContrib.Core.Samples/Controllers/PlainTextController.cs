using Microsoft.AspNetCore.Mvc;

namespace WebApiContrib.Core.Samples.Controllers
{
    [Route("api/[controller]")]
    public class PlainTextController : Controller
    {
        [HttpPost]
        [Produces("text/csv")]
        public string Post([FromBody]string value)
        {
            return value;
        }
    }
}