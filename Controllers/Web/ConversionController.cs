using Microsoft.AspNetCore.Mvc;
using netscii.Constants;
using netscii.Models.ViewModels;
using netscii.Services;
using netscii.Services.Factories;
using netscii.Utils.ImageConverters.Models;


namespace netscii.Controllers.Web
{
    [Route("convert/{format}")]
    public class ConversionController : BaseController
    {
        private readonly ConversionViewModelFactory _viewModelFactory;

        public ConversionController(ConversionService conversionService, ConversionViewModelFactory viewModelFactory) : base(conversionService)
        {
            _viewModelFactory = viewModelFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromRoute] string format)
        {
            if (IsUnsupportedFormat(format))
                return ErrorResponse(400, ExceptionMessages.UnsupportedFormat);

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
                return ErrorResponse(400, ExceptionMessages.UnsupportedFormat);

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
