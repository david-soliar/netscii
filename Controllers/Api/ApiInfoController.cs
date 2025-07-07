using Microsoft.AspNetCore.Mvc;
using netscii.Services;


namespace netscii.Controllers.Api
{
    [ApiController]
    [Route("api")]
    [Produces("application/json")]
    public class ApiInfoController : BaseConversionController
    {
        private readonly ConversionService _conversionService;
        private readonly FontService _fontService;
        private readonly ColorService _colorService;

        public ApiInfoController(ConversionService conversionService, FontService fontService, ColorService colorService) : base(conversionService)
        {
            _conversionService = conversionService;
            _fontService = fontService;
            _colorService = colorService;
        }

        [HttpGet("fonts/{format}")]
        public async Task<IActionResult> GetFontsByFormat(string format)
        {
            if (IsUnsupportedFormat(format))
                return ErrorResponse(400, "Unsupported format");

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

        [HttpGet("colors")]
        public async Task<IActionResult> GetColors()
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _colorService.GetColorsAsync();
                return Ok(result);
            });
        }
    }
}
