using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System.Text;
using netscii.Utils.ImageConverters.Models;
using SixLabors.ImageSharp.Advanced;

namespace netscii.Utils.ImageConverters.Converters
{
    public class SvgConverter
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
                options.Font = "monospace";


            var svg = new StringBuilder();

            var memoryGroup = image.GetPixelMemoryGroup();

            var pixelMemory = memoryGroup[0];
            var pixels = pixelMemory.Span;

            svg.AppendLine($"<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 {width / options.Scale * 24 - 8} {height / options.Scale * 32}\" width=\"100%\" height=\"100%\" font-family=\"{options.Font}\" font-size=\"32\" preserveAspectRatio=\"xMinYMin meet\">");

            string bg = options.UseBackgroundColor ? options.Background : "transparent";
            svg.AppendLine($"<rect width=\"100%\" height=\"100%\" fill=\"{bg}\" />");

            int ySVG = 24;
            for (int y = 0; y < height; y += options.Scale)
            {
                svg.AppendLine($"<text y=\"{ySVG}\">");
                ySVG += 32;
                int xSVG = 0;
                for (int x = 0; x < width; x += options.Scale)
                {
                    int index = y * width + x;

                    Rgba32 pixel = pixels[index];
                    if (options.Invert)
                        pixel = ConverterHelpers.InvertPixel(pixel);

                    int charIndex = ConverterHelpers.GetCharIndex(pixel, options.Characters.Length);

                    string hex = $"{pixel.R:X2}{pixel.G:X2}{pixel.B:X2}";
                    svg.AppendLine($"<tspan x=\"{xSVG}\" fill=\"#{hex}\">{options.Characters[charIndex]}</tspan>");
                    xSVG += 24;
                }
                svg.AppendLine("</text>");
            }
            svg.AppendLine("</svg>");
            return svg.ToString();
        }
    }
}
