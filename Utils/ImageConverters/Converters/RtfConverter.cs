using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System.Text;
using SixLabors.ImageSharp.Advanced;
using netscii.Utils.ImageConverters.Models;
using netscii.Utils.ImageConverters.Exceptions;
using SixLabors.ImageSharp.Processing;
using System.Linq;


namespace netscii.Utils.ImageConverters.Converters
{
    public class RtfConverter
    {
        public static ConverterResult Convert(Stream imageStream, ConverterOptions options)
        {
            using Image<Rgba32> image = Image.Load<Rgba32>(imageStream);


            if (image == null)
                throw new ConverterException(ConverterErrorCode.ImageLoadFailed);

            if (options.Scale <= 0 || options.Scale >= image.Width || options.Scale >= image.Height)
                throw new ConverterException(ConverterErrorCode.InvalidScale);

            if (string.IsNullOrEmpty(options.Characters))
                options.Characters = "A_";

            if (string.IsNullOrEmpty(options.Font))
                options.Font = "Consolas";


            image.Mutate(x => x.Resize(image.Width / options.Scale, image.Height / options.Scale));

            var result = new ConverterResult { Width = image.Width, Height = image.Height };

            var head = new StringBuilder();
            var text = new StringBuilder();

            var definedColors = new List<Rgba32>();

            int x = 0;
            var memoryGroup = image.GetPixelMemoryGroup();

            foreach (var memory in memoryGroup)
            {
                var pixels = memory.Span;

                for (int i = 0; i < pixels.Length;)
                {
                    Rgba32 pixel = pixels[i];
                    if (options.Invert)
                        pixel = ConverterHelpers.InvertPixel(pixel);

                    int charIndex = ConverterHelpers.GetCharIndex(pixel, options.Characters.Length);

                    if (!definedColors.Contains(pixel))
                    {
                        definedColors.Add(pixel);
                    }

                    int indexOfColor = definedColors.IndexOf(pixel);

                    text.Append($"\\cf{indexOfColor + 1} {options.Characters[charIndex]} ");

                    i += 1;
                    x += 1;

                    if (x >= image.Width)
                    {
                        x = 0;
                        text.AppendLine("\\line");
                    }
                }
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
            if (options.UseBackgroundColor && !string.IsNullOrWhiteSpace(options.Background))
            {
                var (r, g, b) = ConverterHelpers.HexToRGB("#FFAA33");
                bg = new Rgba32(r, g, b);
                if (!definedColors.Contains(bg))
                {
                    definedColors.Add(bg);
                }
            }

            head.Append("\n}\n");

            if (options.UseBackgroundColor && !string.IsNullOrWhiteSpace(options.Background))
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
