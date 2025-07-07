using Microsoft.AspNetCore.Mvc;
using netscii.Models.ViewModels;
using netscii.Services;
using netscii.Utils.ImageConverters.Exceptions;

namespace netscii.Controllers.Api
{
    public abstract class BaseConversionController : Controller
    {
        protected readonly List<string> _formats;

        protected BaseConversionController(ConversionService conversionService)
        {
            _formats = conversionService.SupportedFormats();
        }

        protected bool IsUnsupportedFormat(string format) => !_formats.Contains(format);

        protected async Task<IActionResult> ExecuteSafe(Func<Task<IActionResult>> operation, int errorCode = 500, string errorMessage = "Internal server error", bool renderErrorView = false)
        {
            try
            {
                return await operation();
            }
            catch (ConverterException ex)
            {
                return ErrorResponse(400, ex.Message, renderErrorView);
            }
            catch
            {
                return ErrorResponse(errorCode, errorMessage, renderErrorView);
            }
        }

        protected IActionResult ErrorResponse(int errorCode, string message, bool renderErrorView = false)
        {
            var error = new ErrorViewModel { Code = errorCode, Message = message };

            Response.StatusCode = errorCode;

            if (renderErrorView)
                return View("Error", error);

            return StatusCode(errorCode, error);
        }
    }
}
