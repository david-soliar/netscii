using Microsoft.AspNetCore.Mvc;
using netscii.Controllers.Api;
using netscii.Models.ViewModels;
using netscii.Services;
using netscii.Services.Factories;
using netscii.Utils.ImageConverters.Models;


namespace netscii.Controllers.Web
{
    [Route("convert/{format}")]
    public class ConversionController : BaseConversionController
    {
        private readonly ConversionService _conversionService;
        private readonly ConversionViewModelFactory _viewModelFactory;

        public ConversionController(ConversionService conversionService, ConversionViewModelFactory viewModelFactory) : base(conversionService)
        {
            _conversionService = conversionService;
            _viewModelFactory = viewModelFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromRoute] string format)
        {
            if (IsUnsupportedFormat(format))
                return ErrorResponse(400, "Unsupported format");

            return await ExecuteSafe(async () =>
                {
                    var model = await _viewModelFactory.CreateWithDefaults(format);
                    return View(model);
                }, 
                renderErrorView: true
            );
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromRoute] string format, ConversionViewModel model)
        {
            if (IsUnsupportedFormat(format))
                return ErrorResponse(400, "Unsupported format");

            return await ExecuteSafe(async () =>
                {
                    ConverterResult result = await _conversionService.ConvertAsync(format, model);
                    model.Result = result.Content;
                    await _viewModelFactory.Repopulate(model, format);

                    return View(model);
                }, 
                renderErrorView: true
            );
        }
    }
}
