using Microsoft.AspNetCore.Mvc;


namespace netscii.Controllers
{
    [ApiController]
    [Route("api/")]
    public class ConvertApiController : Controller
    {
        [HttpPost("html")]
        public async Task<IActionResult> ConvertToHTML([FromForm] Models.FormRequest request)
        {
            if (request.IsInvalid())
                return BadRequest(request.Status);

            using var stream = request.GetImageStream();

            var result = await Task.Run( () => 
                Utils.Converter.HTML(
                    stream,
                    request.Characters,
                    request.Scale,
                    request.Invert,
                    request.Font,
                    request.Background));

            return Content(result, "text/html");
        }

        [HttpPost("md")]
        public async Task<IActionResult> ConvertToMD([FromForm] Models.FormRequest request)
        {
            if (request.IsInvalid())
                return BadRequest(request.Status);

            using var stream = request.GetImageStream();

            var result = await Task.Run(() => 
                Utils.Converter.MD(
                    stream,
                    request.Characters,
                    request.Scale,
                    request.Invert,
                    request.Font,
                    request.Background));

            return Content(result, "text/md");
        }

        [HttpPost("ansi")]
        public async Task<IActionResult> ConvertToANSI([FromForm] Models.FormRequest request)
        {
            if (request.IsInvalid())
                return BadRequest(request.Status);

            using var stream = request.GetImageStream();

            var result = await Task.Run(() => 
                Utils.Converter.ANSI(
                    stream,
                    request.Characters,
                    request.Scale,
                    request.Invert,
                    request.Font,
                    request.Background));

            return Content(result, "text/md");
        }

        [HttpPost("latex")]
        public async Task<IActionResult> ConvertToLATEX([FromForm] Models.FormRequest request)
        {
            if (request.IsInvalid())
                return BadRequest(request.Status);

            using var stream = request.GetImageStream();

            var result = await Task.Run(() => 
                Utils.Converter.LATEX(
                    stream,
                    request.Characters,
                    request.Scale,
                    request.Invert,
                    request.Font,
                    request.Background));

            return Content(result, "text/md");
        }

        [HttpPost("rtf")]
        public async Task<IActionResult> ConvertToRTF([FromForm] Models.FormRequest request)
        {
            if (request.IsInvalid())
                return BadRequest(request.Status);

            using var stream = request.GetImageStream();

            var result = await Task.Run(() => 
                Utils.Converter.RTF(
                    stream,
                    request.Characters,
                    request.Scale,
                    request.Invert,
                    request.Font,
                    request.Background));

            return Content(result, "text/md");
        }
    }
}
