using Microsoft.AspNetCore.Mvc;
using WebApiContrib.Core.Samples.Model;

namespace WebApiContrib.Core.Samples.Controllers
{
    [Route("[controller]")]
    public class AddTagHelperDirectiveController : Controller
    {
        public IActionResult Sample()
        {
            return View();
        }
    }
}