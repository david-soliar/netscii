using Microsoft.AspNetCore.Mvc;

namespace netscii.Controllers.Web
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
