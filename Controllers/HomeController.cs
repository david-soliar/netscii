using Microsoft.AspNetCore.Mvc;

namespace netscii.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
