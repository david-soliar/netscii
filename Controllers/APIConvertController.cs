using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Text;

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

            Console.WriteLine("HERE");
            using var stream = request.Image.OpenReadStream();

            var html = XConvertToHtml(stream, request.Characters, request.Scale, request.Invert);

            return Content(html, "text/html");
        }

        public string XConvertToHtml(System.IO.Stream imageStream, string characters, int scale, bool invert)
        {
            using var bitmap = new Bitmap(imageStream);

            var sb = new StringBuilder();

            sb.Append("<pre style='line-height: 80%; font-family: monospace;'>");

            for (int y = 0; y < bitmap.Height; y += scale)
            {
                for (int x = 0; x < bitmap.Width; x += scale)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    int brightness = (pixel.R + pixel.G + pixel.B) / 3;

                    if (invert)
                        brightness = 255 - brightness;

                    int charIndex = brightness * (characters.Length - 1) / 255;

                    sb.Append(characters[charIndex]);
                }
                sb.Append("<br>");
            }

            sb.Append("</pre>");

            return sb.ToString();
        }
    }
}
