using Microsoft.AspNetCore.Mvc;
using netscii.Models.ViewModels;
using netscii.Services.Interfaces;


namespace netscii.Controllers.Web
{
    [Route("{format}")]
    public class ConversionController : Controller
    {
        private readonly IConversionService _conversionService;
        private readonly IConversionViewModelFactory _viewModelFactory;

        public ConversionController(IConversionService conversionService, IConversionViewModelFactory viewModelFactory)
        {
            _conversionService = conversionService;
            _viewModelFactory = viewModelFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromRoute] string format)
        {

            if (!netscii.Constants.SupportedFormats.All.Contains(format))
                return BadRequest(new { error = "Unsupported format" });

            var model = await _viewModelFactory.CreateWithDefaults(format);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromRoute] string format, ConversionViewModel model)
        {
            if (model.IsInvalid())
                return BadRequest(model.Status);

            if (!netscii.Constants.SupportedFormats.All.Contains(format))
                return BadRequest(new { error = "Unsupported format" });

            model.Result = await _conversionService.ConvertAsync(format, model);

            return View(model);
        }
    }
}
