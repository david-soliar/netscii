using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System.Text;
using Microsoft.Extensions.Options;
using netscii.Utils.ImageConverters.Models;
using SixLabors.ImageSharp.Advanced;

namespace netscii.Utils.ImageConverters.Converters
{
    public class AnsiConverter
    {
        public static string Convert(Stream imageStream, ConverterOptions options)
        {
            using Image<Rgba32> image = Image.Load<Rgba32>(imageStream);

            int width = image.Width;
            int height = image.Height;


            if (image == null)
                throw new ArgumentNullException("Could not load the image.");

            if (options.Scale <= 0 || options.Scale >= width || options.Scale >= height)
                throw new ArgumentOutOfRangeException("Scale must be greater than zero and smaller than width and height of the image.");

            if (string.IsNullOrEmpty(options.OperatingSystem))
                throw new ArgumentNullException("Operating system must be specified.");


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
                    throw new ArgumentException("Specified operating system should be from: Windows Terminal/Powershell, Mac/Linux Shell");
            }

            var memoryGroup = image.GetPixelMemoryGroup();

            var pixelMemory = memoryGroup[0];
            var pixels = pixelMemory.Span;

            for (int y = 0; y + options.Scale < height; y += options.Scale * 2)
            {
                for (int x = 0; x < width; x += options.Scale)
                {
                    int indexTop = y * width + x;
                    int indexBottom = (y + options.Scale) * width + x;

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
            return code.ToString();
        }
    }
}
