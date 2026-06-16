using Microsoft.AspNetCore.Mvc;
using netscii.Constants;
using netscii.Models.Dto;
using netscii.Services;

namespace netscii.Controllers.Api
{
    [ApiController]
    [Route("api")]
    [Produces("application/json")]
    public class ApiInfoController : BaseController
    {
        private readonly FontService _fontService;
        private readonly ConversionLoggingService _conversionLoggingService;

        public ApiInfoController(ConversionService conversionService, FontService fontService, ConversionLoggingService conversionLoggingService) : base(conversionService)
        {
            _fontService = fontService;
            _conversionLoggingService = conversionLoggingService;
        }

        [HttpGet("formats")]
        public async Task<IActionResult> GetFormats()
        {
            return await ExecuteSafe(async () =>
            {
                var result = await Task.FromResult<object>(_conversionService.SupportedFormats());
                return Ok(result);
            });
        }

        [HttpGet("fonts/{format}")]
        public async Task<IActionResult> GetFontsByFormat(string format)
        {
            if (IsUnsupportedFormat(format))
                return ErrorResponse(400, ExceptionMessages.UnsupportedFormat);

            return await ExecuteSafe(async () =>
            {
                var fonts = await _fontService.GetFontsByFormatAsync(format);
                return Ok(new Dictionary<string, List<string>> { [format] = fonts });
            });
        }

        [HttpGet("fonts")]
        public async Task<IActionResult> GetFonts()
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _fontService.GetFontsAllAsync();
                return Ok(result);
            });
        }

        [HttpGet("platforms")]
        public async Task<IActionResult> GetPlatforms()
        {
            return await ExecuteSafe(async () =>
            {
                var result = await Task.FromResult<object>(_conversionService.SupportedPlatforms().ToList());
                return Ok(result);
            });
        }

        [HttpGet("log")]
        public async Task<IActionResult> GetLogs([FromQuery] int period)
        {
            return await ExecuteSafe(async () =>
            {
                var logs = await _conversionLoggingService.GetLogsAsync(period);
                return Ok(logs);
            });
        }
    }
}
