using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using netscii.Models;
using netscii.Services;
using netscii.Services.Interfaces;
using System;


namespace netscii.Controllers
{
    [Route("latex")]
    public class LATEXController : BaseController
    {
        public LATEXController(ILATEXConversionService conversionService, NetsciiContext context)
            : base(conversionService, context) { }

        [HttpGet]
        public override async Task<IActionResult> Index()
        {
            var fontsFromDb = await _context.Fonts
                                  .Where(f => f.Format == "HTML") // toto do DB managera
                                  .Select(f => f.Name)
                                  .ToListAsync();
            var model = new ConversionViewModel
            {
                Controller = "LATEX",
                Fonts = fontsFromDb
            };
            return View(model);
        }
    }
}
