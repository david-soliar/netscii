using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using System.Text;


namespace netscii.Utils
{
    public class Converter
    {
        private static void Convert(Stream imageStream, int scale, Action<Rgba32, int> processPixel, Action<int> processRow)
        {
            using Image<Rgba32> image = Image.Load<Rgba32>(imageStream);

            var memoryGroup = image.GetPixelMemoryGroup();

            var pixelMemory = memoryGroup[0];
            var pixels = pixelMemory.Span;

            int width = image.Width;
            int height = image.Height;

            for (int y = 0; y < height; y += scale)
            {
                for (int x = 0; x < width; )
                {
                    int initialX = x;
                    int index = y * width;

                    Rgba32 pixel = pixels[index + x];

                    x += scale;
                    while (x < width && pixel.Equals(pixels[index + x]))
                        x += scale;

                    processPixel(pixel, (x - initialX) / scale);
                }
                processRow(y);
            }
        }

        public static string HTML(Stream imageStream, string characters, int scale, bool invert, string font, string background, bool useBackgroundColor)
        {
            var html = new StringBuilder();
            var css = new StringBuilder();
            var definedColors = new HashSet<string>();

            Action<Rgba32, int> processPixel = (pixel, count) =>
            {
                int brightness = (pixel.R + pixel.G + pixel.B) / 3;
                if (invert)
                    brightness = 255 - brightness;

                int charIndex = brightness * (characters.Length - 1) / 255;

                if (count > 1)
                {
                    Console.Write("optimized pixels: "); Console.Write(count); Console.Write("\n");
                }

                string hex = $"{pixel.R:X2}{pixel.G:X2}{pixel.B:X2}";
                string className = $"c{hex}";
                if (!definedColors.Contains(className))
                {
                    css.AppendFormat(".{0}{{color:#{1};}}\n", className, hex);
                    definedColors.Add(className);
                }

                html.AppendFormat("<span class='{0}'>", className);
                html.Append(count > 1 ? new string(characters[charIndex], count) : characters[charIndex]);
                html.Append("</span>");
            };

            Action<int> processRow = index =>
            {
                html.Append("<br>");
            };

            css.AppendFormat(
                "<style>\n" +
                "#netscii-html-result {{\n" +
                "    line-height: 80%;\n" +
                "    display: inline-block;\n" +
                "    margin: 0 auto;\n" +
                "    overflow: hidden;\n" +
                "    overflow-x: auto;\n" +
                "    font-family: {0};\n" +
                "    background-color: {1};\n" +
                "}}\n",
                font,
                useBackgroundColor ? background : "transparent");


            html.Append("<pre id=\"netscii-html-result\">");

            Convert(imageStream, scale, processPixel, processRow);

            css.Append("</style>");
            html.Append("</pre>");

            return css.Append(html).ToString();
        }

        public static string MD(System.IO.Stream imageStream, string characters, int scale, bool invert, string font, string background, bool useBackgroundColor)
        {
            // Your MD conversion logic here
            return HTML(imageStream, characters, scale, invert, font, background, useBackgroundColor);
        }

        public static string ANSI(System.IO.Stream imageStream, string characters, int scale, bool invert, string font, string background, bool useBackgroundColor)
        {
            // Your ANSI conversion logic here
            return HTML(imageStream, characters, scale, invert, font, background, useBackgroundColor);
        }

        public static string LATEX(System.IO.Stream imageStream, string characters, int scale, bool invert, string font, string background, bool useBackgroundColor)
        {
            // Your LATEX conversion logic here
            return HTML(imageStream, characters, scale, invert, font, background, useBackgroundColor);
        }

        public static string RTF(System.IO.Stream imageStream, string characters, int scale, bool invert, string font, string background, bool useBackgroundColor)
        {
            // Your RTF conversion logic here
            return HTML(imageStream, characters, scale, invert, font, background, useBackgroundColor);
        }
    }
}
