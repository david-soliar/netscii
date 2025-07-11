﻿using Microsoft.AspNetCore.Mvc;
using netscii.Models.ViewModels;
using netscii.Services;
using netscii.Utils.ImageConverters.Exceptions;
using netscii.Constants;

namespace netscii.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly List<string> _formats;
        protected readonly ConversionService _conversionService;

        protected BaseController(ConversionService conversionService)
        {
            _conversionService = conversionService;
            _formats = conversionService.SupportedFormats();
        }

        protected bool IsUnsupportedFormat(string format) => !_formats.Contains(format);

        protected async Task<IActionResult> ExecuteSafe(Func<Task<IActionResult>> operation, int errorCode = 500, string errorMessage = ExceptionMessages.InternalError, bool renderErrorView = false)
        {
            try
            {
                return await operation();
            }
            catch (ConverterException ex)
            {
                Console.WriteLine($"[ERROR] {ex.GetType().Name}: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return ErrorResponse(400, ex.Message, renderErrorView);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {ex.GetType().Name}: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
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
