using netscii.Models;
using netscii.Services.Interfaces;

namespace netscii.Services
{
    public class MDConversionService : IMDConversionService
    {
        public string FormatName => "md";

        public Task<string> ConvertAsync(FormRequest request)
        {
            return Task.Run(() =>
            {
                using var stream = request.GetImageStream();
                return Utils.Converter.MD(
                    stream,
                    request.Characters,
                    request.Scale,
                    request.Invert,
                    request.Font,
                    request.Background,
                    request.UseBackgroundColor,
                    request.CreateFullDocument);
            });
        }
    }
}
