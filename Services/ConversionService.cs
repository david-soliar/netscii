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

        public async Task<ConverterResult> ConvertAsync(string format, JsonConversionViewModel jsonModel)
        {
            byte[] imageBytes = Convert.FromBase64String(jsonModel.Image);
            using var imageStream = new MemoryStream(imageBytes);

            var internalModel = new ConversionViewModel
            {
                Image = new FormFile(imageStream, 0, imageBytes.Length, "Image", "upload.image"),
                Characters = jsonModel.Characters,
                Scale = jsonModel.Scale,
                Invert = jsonModel.Invert,
                Font = jsonModel.Font,
                Background = jsonModel.Background,
                UseBackgroundColor = !string.IsNullOrWhiteSpace(jsonModel.Background),
                CreateFullDocument = jsonModel.CreateFullDocument,
                OperatingSystem = jsonModel.OperatingSystem,
                UseSmallPalette = jsonModel.UseSmallPalette
            };
            return await ConvertAsync(format, internalModel);
        }


        public bool IsUnsupportedFormat(string format)
        {
            return !ConverterDispatcher.Converters.ContainsKey(format);
        }
    }
}
