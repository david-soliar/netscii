using netscii.Models;
using netscii.Services.Interfaces;

namespace netscii.Services
{
    public class TXTConversionService : ITXTConversionService
    {
        public string FormatName => "txt";

        public Task<string> ConvertAsync(FormRequest request)
        {
            return Task.Run(() =>
            {
                using var stream = request.GetImageStream();
                return Utils.Converter.TXT(
                    stream,
                    request.Characters,
                    request.Scale,
                    request.Invert);
            });
        }
    }
}
