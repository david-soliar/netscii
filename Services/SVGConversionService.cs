using netscii.Models;
using netscii.Services.Interfaces;

namespace netscii.Services
{
    public class SVGConversionService : ISVGConversionService
    {
        public string FormatName => "svg";

        public Task<string> ConvertAsync(ConversionViewModel request)
        {
            return Task.Run(() =>
            {
                using var stream = request.GetImageStream();
                return Utils.Converter.SVG(
                    stream,
                    request.Characters,
                    request.Scale,
                    request.Invert,
                    request.Font,
                    request.Background,
                    request.UseBackgroundColor);
            });
        }
    }
}
