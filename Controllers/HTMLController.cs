using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using netscii.Models;
using netscii.Services.Interfaces;
using System;


namespace netscii.Controllers
{
    [Route("html")]
    public class HTMLController : BaseController
    {
        public HTMLController(IHTMLConversionService conversionService, NetsciiContext context)
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
                Controller = "HTML",
                Fonts = fontsFromDb
            };
            model.Font = fontsFromDb.FirstOrDefault(string.Empty);

            return View(model);
        }
    }
}
