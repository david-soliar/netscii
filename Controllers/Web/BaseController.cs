using Microsoft.AspNetCore.Mvc;
using netscii.Models;
using netscii.Models.ViewModels;
using netscii.Services.Interfaces;


namespace netscii.Controllers.Web
{
    public abstract class BaseController : Controller
    {
        protected readonly IConversionService _conversionService;
        protected readonly IConversionViewModelFactory _viewModelFactory;

        public BaseController(IConversionService conversionService, IConversionViewModelFactory viewModelFactory)
        {
            _conversionService = conversionService;
            _viewModelFactory = viewModelFactory;
        }

        [HttpGet]
        public virtual async Task<IActionResult> Index()
        {
            string? format = ControllerContext.RouteData.Values["controller"]?.ToString();

            if (string.IsNullOrWhiteSpace(format))
            {
                ViewBag.ErrorMessage = "Unable to determine format from the route.";
                return View("Error"); // asi skor aj status
            }

            await _viewModelFactory.CreateWithDefaults(format.ToLowerInvariant());
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
