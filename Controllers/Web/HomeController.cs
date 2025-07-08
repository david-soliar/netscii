using Microsoft.AspNetCore.Mvc;

namespace netscii.Controllers.Web
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
