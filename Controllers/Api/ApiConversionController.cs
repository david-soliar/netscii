using Microsoft.AspNetCore.Mvc;
using netscii.Models.Dto;
using netscii.Models.ViewModels;
using netscii.Services;


namespace netscii.Controllers.Api
{
    [ApiController]
    [Route("api/convert/{format}")]
    public class ApiConversionController : BaseController
    {

        public ApiConversionController(ConversionService conversionService) : base(conversionService)
        {
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
        public async Task<IActionResult> ConvertJson([FromRoute] string format, [FromBody] JsonConversionDto jsonRequest)
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
