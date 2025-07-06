using netscii.Models.ViewModels;
using netscii.Utils.ImageConverters;
using netscii.Utils.ImageConverters.Models;


namespace netscii.Services
{
    public class ConversionService
    {
        public Task<ConverterResult> ConvertAsync(string format, ConversionViewModel request)
        {
            return Task.Run(() =>
            {
                using var stream = request.GetImageStream();

                var options = new ConverterOptions
                {
                    Characters = request.Characters,
                    Scale = request.Scale,
                    Invert = request.Invert,
                    Font = request.Font,
                    Background = request.Background,
                    UseBackgroundColor = request.UseBackgroundColor,
                    CreateFullDocument = request.CreateFullDocument,
                    OperatingSystem = request.OperatingSystem,
                    UseSmallPalette = request.UseSmallPalette
                };

                return ConverterDispatcher.Convert(format, stream, options);
            });
        }

        public bool IsUnsupportedFormat(string format)
        {
            return !ConverterDispatcher.Converters.ContainsKey(format);
        }
    }
}
