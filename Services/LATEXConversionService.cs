using netscii.Models;
using netscii.Services.Interfaces;

namespace netscii.Services
{
    public class LATEXConversionService : ILATEXConversionService
    {
        public string FormatName => "latex";

        public Task<string> ConvertAsync(FormRequest request)
        {
            return Task.Run(() =>
            {
                using var stream = request.GetImageStream();
                return Utils.Converter.LATEX(
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
