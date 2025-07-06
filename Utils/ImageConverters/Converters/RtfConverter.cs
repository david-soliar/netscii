using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System.Text;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp.Advanced;
using netscii.Utils.ImageConverters.Models;

namespace netscii.Utils.ImageConverters.Converters
{
    public class RtfConverter
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

                    text.Append($"\\cf{indexOfColor + 1} {options.Characters[charIndex]}");
                }
                text.AppendLine("\\line");
            }

            head.AppendLine("{\\rtf1\\ansi\\deff0");
            head.Append("{\\fonttbl{\\f0");
            head.Append(options.Font);
            head.Append(";}}\\n");
            head.Append("{\\colortbl ;");
            foreach (var item in definedColors)
            {
                head.Append($"\\red{item.R}\\green{item.G}\\blue{item.B};");
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

            head.Append("}\n");

            if (options.UseBackgroundColor)
            {
                int indexOfBg = definedColors.IndexOf(bg);
                head.AppendLine($"\\highlight{indexOfBg + 1} ");
            }

            head.Append("\\f0\\sl200\\slmult1\n");
            head.Append(text);

            head.Append("}");
            return head.ToString();
        }
    }
}
