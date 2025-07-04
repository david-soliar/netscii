using Microsoft.AspNetCore.Mvc;
using netscii.Models;
using netscii.Services.Interfaces;

namespace netscii.Controllers
{
    [ApiController]
    [Route("api/")]
    public class ConvertApiController : Controller
    {
        private readonly NetsciiContext _context;
        private readonly Dictionary<string, IConversionService> _conversionServices;

        public ConvertApiController(IEnumerable<IConversionService> conversionServices, NetsciiContext context)
        {
            _conversionServices = conversionServices.ToDictionary(s => s.FormatName, s => s);
            _context = context;
        }

        [HttpPost("{format}")]
        public async Task<IActionResult> Convert([FromRoute] string format, [FromForm] FormRequest request)
        {
            if (request.IsInvalid())
                return BadRequest(request.Status);

            if (!_conversionServices.TryGetValue(format.ToLowerInvariant(), out var service))
                return BadRequest("Unsupported format");

            var result = await service.ConvertAsync(request);

            string contentType = format.ToLowerInvariant() switch
            {
                "html" => "text/html",
                "svg" => "text/markdown", //toto zmenit
                "ansi" => "text/plain",
                "latex" => "application/x-latex",
                "rtf" => "application/rtf",
                _ => "text/plain"
            };

            return Content(result, contentType);
        }
    }
}
