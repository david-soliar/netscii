using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using netscii.Models;
using netscii.Services.Interfaces;
using System;


namespace netscii.Controllers
{
    [Route("txt")]
    public class TXTController : BaseController
    {
        public TXTController(ITXTConversionService conversionService, NetsciiContext context)
            : base(conversionService, context) { }

        [HttpGet]
        public override async Task<IActionResult> Index()
        {
            var model = new ConversionViewModel
            {
                Controller = "TXT"
            };
            return View(model);
        }
    }
}
