using Microsoft.AspNetCore.Mvc;
using netscii.Models.ViewModels;
using netscii.Services;
using netscii.Utils.ImageConverters.Exceptions;


namespace netscii.Controllers.Api
{
    [ApiController]
    [Route("api/{format}")]
    public class ConvertApiController : Controller
    {
        private readonly ConversionService _conversionService;

        public ConvertApiController(ConversionService conversionService)
        {
            _conversionService = conversionService;
        }

        [HttpPost]
        public async Task<IActionResult> Convert([FromRoute] string format, [FromForm] ConversionViewModel request)
        {
            if (_conversionService.IsUnsupportedFormat(format))
                return BadRequest(new ErrorViewModel { Code = 400, Message = "Unsupported format" });

            try
            {
                var result = await _conversionService.ConvertAsync(format, request);
                return Ok(result);
            }
            catch (ConverterException ex)
            {
                return BadRequest(new ErrorViewModel { Code = 400, Message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new ErrorViewModel { Code = 500, Message = "Internal server error" });
            }
        }
    }
}
