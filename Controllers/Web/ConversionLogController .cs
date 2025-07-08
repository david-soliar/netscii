using Microsoft.AspNetCore.Mvc;
using netscii.Models.ViewModels;
using netscii.Services;


namespace netscii.Controllers.Web
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("log")]
    public class ConversionLogController : BaseController
    {
        private readonly ConversionLoggingService _conversionLoggingService;

        public ConversionLogController(ConversionLoggingService conversionLoggingService, ConversionService conversionService) : base(conversionService)
        {
            _conversionLoggingService = conversionLoggingService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(LogViewModel model)
        {
            return await ExecuteSafe(async () =>
                {
                    var logs = await _conversionLoggingService.GetLogsAsync(model.Period);

                    model.Logs = logs;
                    return View(model);
                },
                renderErrorView: true
            );
        }
    }
}
