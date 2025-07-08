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
        private readonly ColorService _colorService;
        private readonly ConversionLoggingService _conversionLoggingService;

        public ApiInfoController(ConversionService conversionService, FontService fontService, ColorService colorService, ConversionLoggingService conversionLoggingService) : base(conversionService)
        {
            _fontService = fontService;
            _colorService = colorService;
            _conversionLoggingService = conversionLoggingService;
        }

        /// <summary>
        /// Gets fonts supported by the specified format.
        /// </summary>
        /// <param name="format">The font format to filter by.</param>
        /// <returns>List of fonts for the format.</returns>
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

        /// <summary>
        /// Gets all supported fonts.
        /// </summary>
        /// <returns>Dictionary of all fonts grouped by format.</returns>
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

        [HttpGet("colors")]
        public async Task<IActionResult> GetColors()
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _colorService.GetColorsAsync();
                return Ok(result);
            });
        }

        [HttpGet("log")]
        public async Task<IActionResult> GetLogs([FromBody] JsonConversionDto jsonRequest)
        {
            return await ExecuteSafe(async () =>
            {
                var logs = await _conversionLoggingService.GetLogsAsync(jsonRequest.Period);
                return Ok(logs);
            });
        }
    }
}
