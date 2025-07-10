using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System.Text;
using netscii.Utils.ImageConverters.Models;
using SixLabors.ImageSharp.Advanced;
using netscii.Utils.ImageConverters.Exceptions;
using SixLabors.ImageSharp.Processing;


namespace netscii.Utils.ImageConverters.Converters
{
    public class SvgConverter
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
                options.Font = "monospace";


            image.Mutate(x => x.Resize(image.Width / options.Scale, image.Height / options.Scale));

            var result = new ConverterResult { Width = image.Width, Height = image.Height };


            var svg = new StringBuilder();

            svg.AppendLine($"<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 {image.Width * 24 - 8} {image.Height * 32}\" width=\"100%\" height=\"100%\" font-family=\"{options.Font}\" font-size=\"32\" preserveAspectRatio=\"xMinYMin meet\">");

            string bg = options.UseBackgroundColor && !string.IsNullOrWhiteSpace(options.Background) ? options.Background : "transparent";
            svg.AppendLine($"\t<rect width=\"100%\" height=\"100%\" fill=\"{bg}\" />\n");

            int x = 0;
            int ySVG = 24;
            int xSVG = 0;
            var memoryGroup = image.GetPixelMemoryGroup();

            svg.AppendLine($"\t<text y=\"{ySVG}\">");

            foreach (var memory in memoryGroup)
            {
                var pixels = memory.Span;

                for (int i = 0; i < pixels.Length;)
                {
                    Rgba32 pixel = pixels[i];
                    if (options.Invert)
                        pixel = ConverterHelpers.InvertPixel(pixel);

                    int charIndex = ConverterHelpers.GetCharIndex(pixel, options.Characters.Length);

                    string hex = $"{pixel.R:X2}{pixel.G:X2}{pixel.B:X2}";
                    svg.AppendLine($"\t\t<tspan x=\"{xSVG}\" fill=\"#{hex}\">{options.Characters[charIndex]}</tspan>");

                    i += 1;
                    x += 1;
                    xSVG += 24;

                    if (x >= image.Width)
                    {
                        x = 0;
                        ySVG += 32;
                        xSVG = 0;
                        svg.AppendLine("\t</text>");
                        svg.AppendLine($"\t<text y=\"{ySVG}\">");
                    }
                }
            }
            int n = $"\t<text y=\"{ySVG}\">".Length;
            svg.Remove(svg.Length - n, n);

            svg.AppendLine("</svg>");

            result.Content = svg.ToString();
            return result;
        }
    }
}
