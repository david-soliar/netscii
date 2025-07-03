using netscii.Models;
using netscii.Services.Interfaces;

namespace netscii.Services
{
    public class RTFConversionService : IConversionService
    {
        public string FormatName => "rtf";

        public Task<string> ConvertAsync(FormRequest request)
        {
            return Task.Run(() =>
            {
                using var stream = request.GetImageStream();
                return Utils.Converter.RTF(
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
