using netscii.Models.ViewModels;
using netscii.Services.Interfaces;

namespace netscii.Services
{
    public class HTMLConversionService : IHTMLConversionService
    {
        public string FormatName => "html";

        public Task<string> ConvertAsync(ConversionViewModel request)
        {
            return Task.Run(() =>
            {
                using var stream = request.GetImageStream();
                return Utils.Converter.HTML(
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
