using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System.Text;
using netscii.Utils.ImageConverters.Models;
using SixLabors.ImageSharp.Advanced;
using netscii.Utils.ImageConverters.Exceptions;
using SixLabors.ImageSharp.Processing;


namespace netscii.Utils.ImageConverters.Converters
{
    public class AnsiConverter
    {
        public static ConverterResult Convert(Stream imageStream, ConverterOptions options)
        {
            using Image<Rgba32> image = Image.Load<Rgba32>(imageStream);


            if (image == null)
                throw new ConverterException(ConverterErrorCode.ImageLoadFailed);

            if (options.Scale <= 0 || options.Scale >= image.Width || options.Scale >= image.Height)
                throw new ConverterException(ConverterErrorCode.InvalidScale);

            if (string.IsNullOrEmpty(options.Platform))
                throw new ConverterException(ConverterErrorCode.UnsupportedPlatform);


            image.Mutate(x => x.Resize(image.Width / options.Scale, image.Height / (options.Scale * 2)));

            var result = new ConverterResult { Width = image.Width, Height = image.Height };

            var code = new StringBuilder();

            string escape;
            string newLine;

            switch (options.Platform)
            {
                case "Windows Console":
                    escape = "$([char]27)";
                    newLine = "`n";
                    code.Append("Write-Host \"");
                    break;
                case "Unix-like Shell":
                    escape = "\\e";
                    newLine = "\\n";
                    code.Append("printf \"");
                    break;
                default:
                    throw new ConverterException(ConverterErrorCode.UnsupportedPlatform);
            }


            int totalLength = 0;
            foreach (var memory in image.GetPixelMemoryGroup())
                totalLength += memory.Length;

            Span<Rgba32> allPixels = new Rgba32[totalLength];
            int offset = 0;
            foreach (var memory in image.GetPixelMemoryGroup())
            {
                memory.Span.CopyTo(allPixels.Slice(offset));
                offset += memory.Length;
            }

            for (int y = 0; y < image.Height; y += 2)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    int topIndex = y * image.Width + x;
                    int bottomIndex = (y + 1) < image.Height ? (y + 1) * image.Width + x : -1;

                    Rgba32 pixelTop = allPixels[topIndex];
                    Rgba32 pixelBottom = bottomIndex != -1 ? allPixels[bottomIndex] : new Rgba32(0, 0, 0);

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
