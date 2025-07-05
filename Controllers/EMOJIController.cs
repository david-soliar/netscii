using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using netscii.Models;
using netscii.Services.Interfaces;
using System;


namespace netscii.Controllers
{
    [Route("emoji")]
    public class EMOJIController : BaseController
    {
        public EMOJIController(IEMOJIConversionService conversionService, NetsciiContext context)
            : base(conversionService, context) { }

        [HttpGet]
        public override async Task<IActionResult> Index()
        {
            var model = new ConversionViewModel
            {
                Controller = "EMOJI"
            };
            return View(model);
        }
    }
}
