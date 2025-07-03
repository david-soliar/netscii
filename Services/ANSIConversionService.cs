using netscii.Models;
using netscii.Services.Interfaces;

namespace netscii.Services
{
    public class ANSIConversionService : IConversionService
    {
        public string FormatName => "ansi";

        public Task<string> ConvertAsync(FormRequest request)
        {
            return Task.Run(() =>
            {
                using var stream = request.GetImageStream();
                return Utils.Converter.ANSI(
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
