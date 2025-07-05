using netscii.Models;
using netscii.Services.Interfaces;

namespace netscii.Services
{
    public class ANSIConversionService : IANSIConversionService
    {
        public string FormatName => "ansi";

        public Task<string> ConvertAsync(ConversionViewModel request)
        {
            return Task.Run(() =>
            {
                using var stream = request.GetImageStream();
                return Utils.Converter.ANSI(
                    stream,
                    request.Scale,
                    request.Invert,
                    request.OperatingSystem,
                    request.UseSmallPalette);
            });
        }
    }
}
