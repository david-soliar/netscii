using Microsoft.AspNetCore.Mvc;
using netscii.Models.ViewModels;
using netscii.Services.Interfaces;

namespace netscii.Controllers.Api
{
    [ApiController]
    [Route("api/")]
    public class ConvertApiController : Controller
    {
        private readonly IConversionService _conversionService;

        public ConvertApiController(IConversionService conversionService)
        {
            _conversionService = conversionService;
        }

        [HttpPost("{format}")]
        public async Task<IActionResult> Convert([FromRoute] string format, [FromForm] ConversionViewModel request)
        {
            if (request.IsInvalid())
                return BadRequest(new { error = request.Status });

            if (!netscii.Constants.SupportedFormats.All.Contains(format))
                return BadRequest(new { error = "Unsupported format" });

            var result = await _conversionService.ConvertAsync(format, request);

            return Ok(new
            {
                format,
                content = result
            });
        }
    }
}
