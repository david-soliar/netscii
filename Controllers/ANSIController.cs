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
            // tu z db OS ktore viem spravit
            var model = new ConversionViewModel
            {
                Controller = "ANSI"
            };
            return View(model);
        }
    }
}
