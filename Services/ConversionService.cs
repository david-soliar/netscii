using netscii.Models.ViewModels;
using netscii.Services.Interfaces;

namespace netscii.Services
{
    public class ConversionService : IConversionService
    {
        public Task<string> ConvertAsync(string format, ConversionViewModel request)
        {
            return Task.Run(() =>
            {
                using var stream = request.GetImageStream();

                return format switch
                {
                    "emoji" => Utils.Converter.EMOJI(
                        stream,
                        request.Scale,
                        request.Invert),

                    "ansi" => Utils.Converter.ANSI(
                        stream,
                        request.Scale,
                        request.Invert,
                        request.OperatingSystem,
                        request.UseSmallPalette),

                    "svg" => Utils.Converter.SVG(
                        stream,
                        request.Characters,
                        request.Scale,
                        request.Invert,
                        request.Font,
                        request.Background,
                        request.UseBackgroundColor),

                    "html" => Utils.Converter.HTML(
                        stream,
                        request.Characters,
                        request.Scale,
                        request.Invert,
                        request.Font,
                        request.Background,
                        request.UseBackgroundColor),

                    "latex" => Utils.Converter.LATEX(
                        stream,
                        request.Characters,
                        request.Scale,
                        request.Invert,
                        request.Font,
                        request.Background,
                        request.UseBackgroundColor,
                        request.CreateFullDocument),

                    "rtf" => Utils.Converter.RTF(
                        stream,
                        request.Characters,
                        request.Scale,
                        request.Invert,
                        request.Font,
                        request.Background,
                        request.UseBackgroundColor),

                    "txt" => Utils.Converter.TXT(
                        stream,
                        request.Characters,
                        request.Scale,
                        request.Invert),

                    _ => throw new NotSupportedException($"Unsupported format: {format}") // nieco lepsie tuto
                };
            });
        }
    }
}
