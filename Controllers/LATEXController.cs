using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using netscii.Models;
using netscii.Services;
using netscii.Services.Interfaces;
using System;


namespace netscii.Controllers
{
    [Route("tex")]
    public class LATEXController : Controller
    {
        private readonly NetsciiContext _context;
        private readonly IConversionService _latexConversionService;

        public LATEXController(ILATEXConversionService latexConversionService, NetsciiContext context)
        {
            _latexConversionService = latexConversionService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var fontsFromDb = await _context.Fonts
                                  .Where(f => f.Format == "MD") //toto ma byt v DB managerovi
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

            var result = await _latexConversionService.ConvertAsync(request);

            ViewBag.Characters = request.Characters;
            ViewBag.Scale = request.Scale;
            ViewBag.Invert = request.Invert;
            ViewBag.Result = result;
            ViewBag.Background = request.Background ?? "#FFFFFF";
            ViewBag.UseBackgroundColor = request.UseBackgroundColor;
            ViewBag.CreateFullDocument = request.CreateFullDocument;
            ViewBag.Font = request.Font;

            var fontsFromDb = await _context.Fonts
                      .Where(f => f.Format == "LATEX")
                      .Select(f => f.Name)
                      .ToListAsync();

            ViewBag.Fonts = fontsFromDb;

            return View();
        }
    }
}
