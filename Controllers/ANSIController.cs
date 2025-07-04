using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using netscii.Models;
using netscii.Services;
using netscii.Services.Interfaces;
using System;


namespace netscii.Controllers
{
    [Route("ansi")]
    public class ANSIController : Controller
    {
        private readonly NetsciiContext _context;
        private readonly IConversionService _ansiConversionService;

        public ANSIController(IANSIConversionService ansiConversionService, NetsciiContext context)
        {
            _ansiConversionService = ansiConversionService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // tu z db OS ktore viem spravit
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] FormRequest request)
        {
            if (request.IsInvalid())
                return BadRequest(request.Status);

            var result = await _ansiConversionService.ConvertAsync(request);

            ViewBag.Scale = request.Scale;
            ViewBag.Invert = request.Invert;
            ViewBag.Result = result;
            ViewBag.OperatingSystem = request.OperatingSystem;
            ViewBag.UseSmallPalette = request.UseSmallPalette;

            // z db OSes

            return View();
        }
    }
}
