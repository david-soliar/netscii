using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System.Text;
using SixLabors.ImageSharp.Advanced;
using netscii.Utils.ImageConverters.Models;
using netscii.Utils.ImageConverters.Exceptions;


namespace netscii.Utils.ImageConverters.Converters
{
    public class RtfConverter
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


            if (string.IsNullOrEmpty(options.Characters))
                options.Characters = "Aa";

            if (string.IsNullOrEmpty(options.Font))
                options.Font = "Consolas";


            var head = new StringBuilder();
            var text = new StringBuilder();

            var memoryGroup = image.GetPixelMemoryGroup();

            var pixelMemory = memoryGroup[0];
            var pixels = pixelMemory.Span;

            var definedColors = new List<Rgba32>();

            for (int y = 0; y < height; y += options.Scale)
            {
                for (int x = 0; x < width; x += options.Scale)
                {
                    int index = y * width + x;

                    Rgba32 pixel = pixels[index];

                    if (options.Invert)
                        pixel = ConverterHelpers.InvertPixel(pixel);

                    int charIndex = ConverterHelpers.GetCharIndex(pixel, options.Characters.Length);

                    if (!definedColors.Contains(pixel))
                    {
                        definedColors.Add(pixel);
                    }

                    int indexOfColor = definedColors.IndexOf(pixel);

                    text.Append($"\\cf{indexOfColor + 1} {options.Characters[charIndex]} ");
                }
                text.AppendLine("\\line");
            }

            head.AppendLine("{\\rtf1\\ansi\\deff0");
            head.Append("{\\fonttbl{\\f0");
            head.Append(options.Font);
            head.Append(";}}\n");
            head.Append("{\\colortbl ;");
            foreach (var item in definedColors)
            {
                head.Append($"\n\t\\red{item.R}\\green{item.G}\\blue{item.B};");
            }
            Rgba32 bg = new Rgba32();
            if (options.UseBackgroundColor)
            {
                var (r, g, b) = ConverterHelpers.HexToRGB("#FFAA33");
                bg = new Rgba32(r, g, b);
                if (!definedColors.Contains(bg))
                {
                    definedColors.Add(bg);
                }
            }

            head.Append("\n}\n");

            if (options.UseBackgroundColor)
            {
                int indexOfBg = definedColors.IndexOf(bg);
                head.AppendLine($"\\highlight{indexOfBg + 1} ");
            }

            head.AppendLine("\\f0\\sl200\\slmult1\n");
            head.Append(text);

            head.Append("}");

            result.Content = head.ToString();
            return result;
        }
    }
}
