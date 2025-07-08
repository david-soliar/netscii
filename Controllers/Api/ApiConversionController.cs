using Microsoft.AspNetCore.Mvc;
using netscii.Constants;
using netscii.Models.Dto;
using netscii.Models.ViewModels;
using netscii.Services;

namespace netscii.Controllers.Api
{
    [ApiController]
    [Route("api/convert/{format}")]
    public class ApiConversionController : BaseController
    {

        public ApiConversionController(ConversionService conversionService) : base(conversionService) { }

        [HttpPost("form")]
        public async Task<IActionResult> ConvertForm([FromRoute] string format, [FromForm] ConversionViewModel request)
        {
            if (IsUnsupportedFormat(format))
                return ErrorResponse(400, ExceptionMessages.UnsupportedFormat);

            return await ExecuteSafe(async () =>
            {
                var result = await _conversionService.ConvertAsync(format, request);
                return Ok(result);
            });
        }

        [HttpPost("json")]
        [Consumes("application/json")]
        public async Task<IActionResult> ConvertJson([FromRoute] string format, [FromBody] JsonConversionDto jsonRequest)
        {
            if (IsUnsupportedFormat(format))
                return ErrorResponse(400, ExceptionMessages.UnsupportedFormat);

            if (string.IsNullOrEmpty(jsonRequest.Image))
                return ErrorResponse(400, ExceptionMessages.ImageRequired);

            return await ExecuteSafe(async () =>
            {
                var result = await _conversionService.ConvertAsync(format, jsonRequest);
                return Ok(result);
            });
        }
    }
}
