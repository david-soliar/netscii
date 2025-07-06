using Microsoft.AspNetCore.Mvc;
using netscii.Models.ViewModels;
using netscii.Services;
using netscii.Services.Factories;
using netscii.Utils.ImageConverters.Exceptions;
using netscii.Utils.ImageConverters.Models;


namespace netscii.Controllers.Web
{
    [Route("convert/{format}")]
    public class ConversionController : Controller
    {
        private readonly ConversionService _conversionService;
        private readonly ConversionViewModelFactory _viewModelFactory;

        public ConversionController(ConversionService conversionService, ConversionViewModelFactory viewModelFactory)
        {
            _conversionService = conversionService;
            _viewModelFactory = viewModelFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromRoute] string format)
        {
            if (_conversionService.IsUnsupportedFormat(format))
                return View("Error", new ErrorViewModel { Code = 400, Message = "Unsupported format" });

            var model = await _viewModelFactory.CreateWithDefaults(format);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromRoute] string format, ConversionViewModel model)
        {
            if (_conversionService.IsUnsupportedFormat(format))
                return View("Error", new ErrorViewModel { Code = 400, Message = "Unsupported format" });

            try
            {
                ConverterResult result = await _conversionService.ConvertAsync(format, model);
                model.Result = result.Content;
                await _viewModelFactory.Repopulate(model, format);

                return View(model);
            }
            catch (ConverterException ex)
            {
                return View("Error", new ErrorViewModel { Code = 400, Message = ex.Message });
            }
            catch
            {
                return View("Error", new ErrorViewModel { Code = 400, Message = "Internal server error" });
            }
        }
    }
}
