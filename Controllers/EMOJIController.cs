using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using netscii.Models;
using netscii.Services.Interfaces;
using System;


namespace netscii.Controllers
{
    [Route("emoji")]
    public class EMOJIController : Controller
    {
        private readonly NetsciiContext _context;
        private readonly IConversionService _emojiConversionService;

        public EMOJIController(IEMOJIConversionService emojiConversionService, NetsciiContext context)
        {
            _emojiConversionService = emojiConversionService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] FormRequest request)
        {
            if (request.IsInvalid())
                return BadRequest(request.Status);

            var result = await _emojiConversionService.ConvertAsync(request);

            ViewBag.Characters = request.Characters;
            ViewBag.Scale = request.Scale;
            ViewBag.Invert = request.Invert;
            ViewBag.Result = result;

            return View();
        }
    }
}
