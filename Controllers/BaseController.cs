using Microsoft.AspNetCore.Mvc;
using netscii.Models;
using netscii.Services.Interfaces;


namespace netscii.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly NetsciiContext _context;
        protected readonly IConversionService _conversionService;

        public BaseController(IConversionService conversionService, NetsciiContext context)
        {
            _conversionService = conversionService;
            _context = context;
        }

        [HttpGet]
        public virtual async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public virtual async Task<IActionResult> Index(ConversionViewModel model)
        {
            if (model.IsInvalid())
                return BadRequest(model.Status);

            model.Result = await _conversionService.ConvertAsync(model);

            return View(model);
        }
    }
}
