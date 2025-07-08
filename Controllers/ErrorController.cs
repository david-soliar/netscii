using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using netscii.Constants;
using netscii.Models.ViewModels;

namespace netscii.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Error")]
        public IActionResult HandleError()
        {
            var model = new ErrorViewModel
            {
                Code = 500,
                Message = ExceptionMessages.Error,
            };

            return View("Error", model);
        }

        [Route("/Error/{code:int}")]
        public IActionResult HandleStatusCode(int code)
        {
            var model = new ErrorViewModel
            {
                Code = code,
                Message = ExceptionMessages.Messages.GetValueOrDefault(code, "An unexpected error occurred.")
            };

            return View("Error", model);
        }
    }
}
