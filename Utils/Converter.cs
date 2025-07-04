using Microsoft.Extensions.Primitives;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using System.Text;


namespace netscii.Utils
{
    public class Converter
    {
        private static int[] Convert(Stream imageStream, int scale, Action<Rgba32, int> processPixel, Action<int> processRow)
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
            return new int[] { width / scale, height / scale};
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

        public static string LATEX(Stream imageStream, string characters, int scale, bool invert, string font, string background, bool useBackgroundColor, bool createFullDocument)
        {
            var document = new StringBuilder();
            var tex = new StringBuilder();

            Action<Rgba32, int> processPixel = (pixel, count) =>
            {
                int brightness = (pixel.R + pixel.G + pixel.B) / 3;
                if (invert)
                    brightness = 255 - brightness;
                double r = (invert ? 255 - pixel.R : pixel.R) / 255.0;
                double g = (invert ? 255 - pixel.G : pixel.G) / 255.0;
                double b = (invert ? 255 - pixel.B : pixel.B) / 255.0;


                int charIndex = brightness * (characters.Length - 1) / 255;

                if (count > 1)
                {
                    Console.Write("optimized pixels: "); Console.Write(count); Console.Write("\n");
                }

                tex.AppendFormat("{{\\color[rgb]{{{0},{1},{2}}} {3}}}", 
                    r, g, b, 
                    new string(characters[charIndex], count));
            };

            Action<int> processRow = index =>
            {
                tex.AppendLine(@"\newline").AppendLine();
            };

            if (createFullDocument)
            {
                document.AppendLine("\\documentclass{article}");
                document.AppendLine("\\usepackage{xcolor}");
                document.AppendLine("\\linespread{0}");
            }
            if (useBackgroundColor)
            {
                document.AppendLine("\\definecolor{mybg}{rgb}{0.1,0.2,0.3}");
                document.AppendLine("\\pagecolor{mybg}");
            }

            int[] dimensions = Convert(imageStream, scale, processPixel, processRow);
            if (createFullDocument)
            {
                double charWidthCm = 0.2;
                double charHeightCm = 0.28;
                double paperWidthCm = charWidthCm * dimensions[0];
                double paperHeightCm = charHeightCm * dimensions[1];
                document.AppendLine($"\\usepackage[paperwidth={paperWidthCm}cm, paperheight={paperHeightCm}cm, margin=0pt]{{geometry}}");
            }

            if (createFullDocument)
            {
                document.AppendLine("\\begin{document}");
                document.AppendLine("\\vbox to \\textheight{");
                document.AppendLine("\\vfill");
                document.AppendLine("\\begin{center}");
                document.AppendLine("{\\ttfamily");
                document.AppendLine("\\setlength{\\baselineskip}{0pt}");
            }
            document.Append(tex);
            if (createFullDocument)
            {
                document.AppendLine("}");
                document.AppendLine("\\end{center}").AppendLine();
                document.AppendLine("\\vfill");
                document.AppendLine("}");
                document.AppendLine("\\end{document}").AppendLine();
            }

            return document.ToString();
        }

        public static string ANSI(Stream imageStream, int scale, bool invert, string font, bool useSmallPalette)
        {
            var sb = new StringBuilder();

            string escape = font switch
            {
                "Windows" => "$([char]27)",
                "Linux/Mac" => "\\e",
                _ => string.Empty
            };

            using Image<Rgba32> image = Image.Load<Rgba32>(imageStream);

            var memoryGroup = image.GetPixelMemoryGroup();

            var pixelMemory = memoryGroup[0];
            var pixels = pixelMemory.Span;

            int width = image.Width;
            int height = image.Height;

            for (int y = 0; y + scale < height; y += (scale * 2))
            {
                for (int x = 0; x < width; x += scale)
                {
                    int indexTop = y * width + x;
                    int indexBottom = (y + scale) * width + x;

                    Rgba32 pixelTop = pixels[indexTop];
                    Rgba32 pixelBottom = pixels[indexBottom];

                    if (invert)
                    {
                        pixelTop = new Rgba32(
                            (byte)(255 - pixelTop.R),
                            (byte)(255 - pixelTop.G),
                            (byte)(255 - pixelTop.B),
                            pixelTop.A
                        );
                        pixelBottom = new Rgba32(
                            (byte)(255 - pixelBottom.R),
                            (byte)(255 - pixelBottom.G),
                            (byte)(255 - pixelBottom.B),
                            pixelBottom.A
                        );
                    }

                    string fg = string.Empty;
                    string bg = string.Empty;

                    if (useSmallPalette)
                    {
                        int r = (int)Math.Round(Math.Clamp(pixelTop.R / 255.0 * 5, 0, 5));
                        int g = (int)Math.Round(Math.Clamp(pixelTop.G / 255.0 * 5, 0, 5));
                        int b = (int)Math.Round(Math.Clamp(pixelTop.B / 255.0 * 5, 0, 5));
                        int pixelTopSmall = 16 + 36 * r + 6 * g + b;

                        r = (int)Math.Round(Math.Clamp(pixelBottom.R / 255.0 * 5, 0, 5));
                        g = (int)Math.Round(Math.Clamp(pixelBottom.G / 255.0 * 5, 0, 5));
                        b = (int)Math.Round(Math.Clamp(pixelBottom.B / 255.0 * 5, 0, 5));
                        int pixelBottomSmall = 16 + 36 * r + 6 * g + b;

                        fg = $"{escape}[38;5;{pixelTopSmall};m";
                        bg = $"{escape}[48;5;{pixelBottomSmall};m";
                    }
                    else
                    {
                        fg = $"{escape}[38;2;{pixelTop.R};{pixelTop.G};{pixelTop.B}m";
                        bg = $"{escape}[48;2;{pixelBottom.R};{pixelBottom.G};{pixelBottom.B}m";
                    }

                    sb.Append($"{fg}{bg}▀");
                }
                sb.Append("{escape}[0m\n");
            }
            sb.Append("{escape}[0m");
            return sb.ToString();
        }

        public static string MD(Stream imageStream, string characters, int scale, bool invert, string font, string background, bool useBackgroundColor, bool createFullDocument)
        {
            // Your LATEX conversion logic here
            return HTML(imageStream, characters, scale, invert, font, background, useBackgroundColor);
        }

        public static string RTF(Stream imageStream, string characters, int scale, bool invert, string font, string background, bool useBackgroundColor)
        {
            // Your RTF conversion logic here
            return HTML(imageStream, characters, scale, invert, font, background, useBackgroundColor);
        }
    }
}
