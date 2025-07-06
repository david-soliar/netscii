using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System.Text;
using netscii.Utils.ImageConverters.Models;
using SixLabors.ImageSharp.Advanced;
using netscii.Utils.ImageConverters.Exceptions;


namespace netscii.Utils.ImageConverters.Converters
{
    public class AnsiConverter
    {
        public static ConverterResult Convert(Stream imageStream, ConverterOptions options)
        {
            using Image<Rgba32> image = Image.Load<Rgba32>(imageStream);

            int width = image.Width;
            int height = image.Height;

            var result = new ConverterResult { Width = width, Height = height };


            if (image == null)
                throw new ConverterException(ConverterErrorCode.ImageLoadFailed);

            if (options.Scale <= 0 || options.Scale >= width || options.Scale >= height)
                throw new ConverterException(ConverterErrorCode.InvalidScale);

            if (string.IsNullOrEmpty(options.OperatingSystem))
                throw new ConverterException(ConverterErrorCode.InvalidOperatingSystem);


            var code = new StringBuilder();

            string escape;
            string newLine;

            switch (options.OperatingSystem)
            {
                case "Windows Terminal/Powershell":
                    escape = "$([char]27)";
                    newLine = "`n";
                    code.Append("Write-Host \"");
                    break;
                case "Mac/Linux Shell":
                    escape = "\\e";
                    newLine = "\\n";
                    code.Append("printf \"");
                    break;
                default:
                    throw new ConverterException(ConverterErrorCode.InvalidOperatingSystem);
            }

            var memoryGroup = image.GetPixelMemoryGroup();

            var pixelMemory = memoryGroup[0];
            var pixels = pixelMemory.Span;

            for (int y = options.Scale; y < height; y += (options.Scale * 2))
            {
                for (int x = 0; x < width; x += options.Scale)
                {
                    int indexTop = (y - options.Scale) * width + x;
                    int indexBottom = y * width + x;

                    Rgba32 pixelTop = pixels[indexTop];
                    Rgba32 pixelBottom = pixels[indexBottom];

                    if (options.Invert)
                    {
                        pixelTop = ConverterHelpers.InvertPixel(pixelTop);
                        pixelBottom = ConverterHelpers.InvertPixel(pixelBottom);
                    }

                    string fg, bg;

                    if (options.UseSmallPalette)
                    {
                        fg = $"{escape}[38;5;{ConverterHelpers.To256ColorCode(pixelTop)}m";
                        bg = $"{escape}[48;5;{ConverterHelpers.To256ColorCode(pixelBottom)}m";
                    }
                    else
                    {
                        fg = $"{escape}[38;2;{pixelTop.R};{pixelTop.G};{pixelTop.B}m";
                        bg = $"{escape}[48;2;{pixelBottom.R};{pixelBottom.G};{pixelBottom.B}m";
                    }

                    code.Append($"{fg}{bg}▀");
                }
                code.Append($"{escape}[0m{newLine}");
            }
            code.Append($"{escape}[0m");
            code.Append("\"");

            result.Content = code.ToString();
            return result;
        }
    }
}
