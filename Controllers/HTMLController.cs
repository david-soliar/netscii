using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using netscii.Models;
using netscii.Services.Interfaces;
using System;


namespace netscii.Controllers
{
    [Route("html")]
    public class HTMLController : Controller
    {
        private readonly NetsciiContext _context;
        private readonly IConversionService _htmlConversionService;

        public HTMLController(IConversionService htmlConversionService, NetsciiContext context)
        {
            _htmlConversionService = htmlConversionService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var fontsFromDb = await _context.Fonts
                                  .Where(f => f.Format == "HTML")
                                  .Select(f => f.Name)
                                  .ToListAsync();

            ViewBag.Fonts = fontsFromDb;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] FormRequest request)
        {
            if (request.IsInvalid())
                return BadRequest(request.Status);

            var result = await _htmlConversionService.ConvertAsync(request);

            ViewBag.Characters = request.Characters;
            ViewBag.Scale = request.Scale;
            ViewBag.Invert = request.Invert;
            ViewBag.Result = result;
            ViewBag.Background = request.Background ?? "#FFFFFF";
            ViewBag.UseBackgroundColor = request.UseBackgroundColor;
            ViewBag.Font = request.Font;

            var fontsFromDb = await _context.Fonts
                      .Where(f => f.Format == "HTML")
                      .Select(f => f.Name)
                      .ToListAsync();

            ViewBag.Fonts = fontsFromDb;

            return View();
        }
    }
}
