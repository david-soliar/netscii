using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using System.Text;


namespace netscii.Utils
{
    public class Converter
    {
        private static void Convert(Stream imageStream, int scale, Action<Rgba32> processPixel, Action<int> processRow)
        {
            using Image<Rgba32> image = Image.Load<Rgba32>(imageStream);

            var memoryGroup = image.GetPixelMemoryGroup();

            var pixelMemory = memoryGroup[0];
            var pixels = pixelMemory.Span;

            int width = image.Width;
            int height = image.Height;

            for (int y = 0; y < height; y += scale)
            {
                for (int x = 0; x < width; x += scale)
                {
                    int index = y * width + x;

                    Rgba32 pixel = pixels[index];

                    processPixel(pixel);
                }
                processRow(y);
            }
        }

        public static string HTML(Stream imageStream, string characters, int scale, bool invert, string font, string background)
        {
            var sb = new StringBuilder();

            Action<Rgba32> processPixel = pixel =>
            {
                int brightness = (pixel.R + pixel.G + pixel.B) / 3;
                if (invert)
                    brightness = 255 - brightness;

                int charIndex = brightness * (characters.Length - 1) / 255;

                sb.AppendFormat("<span style='color: rgb({0},{1},{2})'>{3}</span>", pixel.R, pixel.G, pixel.B, characters[charIndex]);
            };

            Action<int> processRow = index =>
            {
                sb.Append("<br>");
            };

            sb.Append("<pre style='line-height: 80%; font-family: monospace;'>");

            Convert(imageStream, scale, processPixel, processRow);

            sb.Append("</pre>");

            return sb.ToString();
        }

        public static string MD(System.IO.Stream imageStream, string characters, int scale, bool invert, string font, string background)
        {
            // Your MD conversion logic here
            return HTML(imageStream, characters, scale, invert, font, background);
        }

        public static string ANSI(System.IO.Stream imageStream, string characters, int scale, bool invert, string font, string background)
        {
            // Your ANSI conversion logic here
            return HTML(imageStream, characters, scale, invert, font, background);
        }

        public static string LATEX(System.IO.Stream imageStream, string characters, int scale, bool invert, string font, string background)
        {
            // Your LATEX conversion logic here
            return HTML(imageStream, characters, scale, invert, font, background);
        }

        public static string RTF(System.IO.Stream imageStream, string characters, int scale, bool invert, string font, string background)
        {
            // Your RTF conversion logic here
            return HTML(imageStream, characters, scale, invert, font, background);
        }
    }
}
