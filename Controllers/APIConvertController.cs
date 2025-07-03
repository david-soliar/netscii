using Microsoft.AspNetCore.Mvc;


public class ConversionFormRequest
{
    public IFormFile Image { get; set; } = null!;
    public string Characters { get; set; } = "@%#*+=-:. ";
    public int Scale { get; set; } = 8;
    public bool Invert { get; set; } = false;
}


namespace netscii.Controllers
{
    [ApiController]
    [Route("api/")]
    public class ConvertApiController : Controller
    {
        [HttpPost("html")]
        public async Task<IActionResult> ConvertToHtml([FromForm] ConversionFormRequest request)
        {
            if (request.Image == null || request.Image.Length == 0)
                return BadRequest("Image file is required.");

            using var stream = request.Image.OpenReadStream();

            var result = await Task.Run( () => Utils.HTMLConverter.Convert(stream, request.Characters, request.Scale, request.Invert));

            return Content(result, "text/html");
        }
    }
}
