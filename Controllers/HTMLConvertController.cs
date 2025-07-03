using Microsoft.AspNetCore.Mvc;


namespace netscii.Controllers
{
    public class HTMLConvertController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
