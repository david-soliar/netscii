using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using netscii.Models;
using netscii.Services;
using netscii.Services.Interfaces;
using System;


namespace netscii.Controllers
{
    [Route("ansi")]
    public class ANSIController : BaseController
    {
        public ANSIController(IANSIConversionService conversionService, NetsciiContext context)
            : base(conversionService, context) { }

        [HttpGet]
        public override async Task<IActionResult> Index()
        {
            var operatingSystemsFromDb = await _context.OperatingSystems
                                  .Select(f => f.Name)
                                  .ToListAsync();

            var model = new ConversionViewModel
            {
                Controller = "ANSI",
                OperatingSystems = operatingSystemsFromDb
            };
            model.OperatingSystem = operatingSystemsFromDb.FirstOrDefault(string.Empty);

            return View(model);
        }
    }
}
