using System.IO;
using Microsoft.Extensions.Options;
using netscii.Models.Dto;
using netscii.Models.ViewModels;
using netscii.Utils.ImageConverters;
using netscii.Utils.ImageConverters.Models;


namespace netscii.Services
{
    public class ConversionService
    {
        private readonly ConversionLoggingService _loggingService;

        public ConversionService(ConversionLoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        private ConverterOptions BuildOptions(dynamic request)
        {
            return new ConverterOptions
            {
                Characters = request.Characters,
                Scale = request.Scale,
                Invert = request.Invert,
                Font = request.Font,
                Background = request.Background,
                UseBackgroundColor = request.UseBackgroundColor,
                Platform = request.Platform,
                UseSmallPalette = request.UseSmallPalette
            };
        }

        public async Task<ConverterResult> ConvertAsync(string format, ConversionViewModel request)
        {
            ConverterOptions options = BuildOptions(request);

            ConverterResult result = await Task.Run(() =>
                ConverterDispatcher.Convert(format, request.GetImageStream(), options)
            );

            await _loggingService.LogAsync(format, result, options);

            return result;
        }

        public async Task<ConverterResult> ConvertAsync(string format, JsonConversionDto request)
        {
            ConverterOptions options = BuildOptions(request);

            ConverterResult result = await Task.Run(() =>
            {
                byte[] imageBytes = Convert.FromBase64String(request.Image);
                return ConverterDispatcher.Convert(format, new MemoryStream(imageBytes), options);
            });

            await _loggingService.LogAsync(format, result, options);

            return result;
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
