using Microsoft.AspNetCore.Mvc;
using netscii.Models;
using System;


namespace netscii.Controllers
{
    [Route("html")]
    public class HTMLConvertController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] FormRequest request)
        {
            if (request.IsInvalid())
                return BadRequest(request.Status);

            using var stream = request.GetImageStream();
            var result = await Task.Run(() =>
                Utils.Converter.HTML(
                    stream,
                    request.Characters,
                    request.Scale,
                    request.Invert,
                    request.Font,
                    request.Background));

            ViewBag.Characters = request.Characters;
            ViewBag.Scale = request.Scale;
            ViewBag.Invert = request.Invert;
            ViewBag.Result = result;
            return View();
        }
    }
}
