using Microsoft.AspNetCore.Mvc;
using netscii.Models.ViewModels;
using netscii.Services;


namespace netscii.Controllers.Api
{
    [ApiController]
    [Route("api/convert/{format}")]
    public class ApiConversionController : BaseConversionController
    {
        private readonly ConversionService _conversionService;

        public ApiConversionController(ConversionService conversionService) : base(conversionService)
        {
            _conversionService = conversionService;
        }

        [HttpPost]
        public async Task<IActionResult> ConvertForm([FromRoute] string format, [FromForm] ConversionViewModel request)
        {
            if (IsUnsupportedFormat(format))
                return ErrorResponse(400, "Unsupported format");

            return await ExecuteSafe(async () =>
            {
                var result = await _conversionService.ConvertAsync(format, request);
                return Ok(result);
            });
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> ConvertJson([FromRoute] string format, [FromBody] JsonConversionViewModel jsonRequest)
        {
            if (IsUnsupportedFormat(format))
                return ErrorResponse(400, "Unsupported format");

            if (string.IsNullOrEmpty(jsonRequest.Image))
                return ErrorResponse(400, "Image is required.");

            return await ExecuteSafe(async () =>
            {
                var result = await _conversionService.ConvertAsync(format, jsonRequest);
                return Ok(result);
            });
        }
    }
}
