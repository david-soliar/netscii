using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using netscii.Models;
using netscii.Services.Interfaces;
using System;


namespace netscii.Controllers
{
    [Route("svg")]
    public class SVGController : BaseController
    {
        public SVGController(ISVGConversionService conversionService, NetsciiContext context)
            : base(conversionService, context) { }

        [HttpGet]
        public override async Task<IActionResult> Index()
        {
            var fontsFromDb = await _context.Fonts
                                  .Where(f => f.Format == "HTML")
                                  .Select(f => f.Name)
                                  .ToListAsync();
            var model = new ConversionViewModel
            {
                Controller = "TXT", Fonts = fontsFromDb
            };
            return View(model);
        }
    }
}
