using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using netscii.Models;
using netscii.Services.Interfaces;
using System;


namespace netscii.Controllers
{
    [Route("rtf")]
    public class RTFController : BaseController
    {
        public RTFController(IRTFConversionService conversionService, NetsciiContext context)
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
                Controller = "RTF",
                Fonts = fontsFromDb
            };
            model.Font = fontsFromDb.FirstOrDefault(string.Empty);

            return View(model);
        }
    }
}
