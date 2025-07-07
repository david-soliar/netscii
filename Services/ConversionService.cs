using System.IO;
using Microsoft.Extensions.Options;
using netscii.Models.ViewModels;
using netscii.Utils.ImageConverters;
using netscii.Utils.ImageConverters.Models;


namespace netscii.Services
{
    public class ConversionService
    {
        public async Task<ConverterResult> ConvertAsync(string format, ConversionViewModel request)
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
                Platform = request.Platform,
                UseSmallPalette = request.UseSmallPalette
            };

            return await Task.FromResult(ConverterDispatcher.Convert(format, stream, options));
        }

        public async Task<ConverterResult> ConvertAsync(string format, JsonConversionViewModel request)
        {
            byte[] imageBytes = Convert.FromBase64String(request.Image);
            using var stream = new MemoryStream(imageBytes);

            var options = new ConverterOptions
            {
                Characters = request.Characters,
                Scale = request.Scale,
                Invert = request.Invert,
                Font = request.Font,
                Background = request.Background,
                UseBackgroundColor = request.UseBackgroundColor,
                CreateFullDocument = request.CreateFullDocument,
                Platform = request.Platform,
                UseSmallPalette = request.UseSmallPalette
            };

            return await Task.FromResult(ConverterDispatcher.Convert(format, stream, options));
        }

        public List<string> SupportedFormats()
        {
            return ConverterDispatcher.Converters.Keys.ToList();
        }

        public IReadOnlyList<string> SupportedPlatforms()
        {
            return ConverterDispatcher.SupportedPlatforms;
        }
    }
}
