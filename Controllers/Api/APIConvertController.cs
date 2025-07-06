using Microsoft.AspNetCore.Mvc;
using netscii.Models;
using netscii.Models.ViewModels;
using netscii.Services.Interfaces;

namespace netscii.Controllers.Api
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
        public async Task<IActionResult> Convert([FromRoute] string format, [FromForm] ConversionViewModel request)
        {
            if (request.IsInvalid())
                return BadRequest(new { error = request.Status });

            if (!_conversionServices.TryGetValue(format.ToLowerInvariant(), out var service))
                return BadRequest(new { error = "Unsupported format" });

            var result = await service.ConvertAsync(request);

            return Ok(new
            {
                format,
                content = result
            });
        }
    }
}
