﻿using SixLabors.ImageSharp;
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

        public static string ANSI(Stream imageStream, int scale, bool invert, string operatingSystem, bool useSmallPalette)
        {
            var sb = new StringBuilder();

            string escape = operatingSystem switch
            {
                "Windows" => "$([char]27)",
                "Linux/Mac" => "\\e",
                _ => string.Empty
            };

            string newLine = operatingSystem switch
            {
                "Windows" => "`n",
                "Linux/Mac" => "\\n",
                _ => string.Empty
            };

            if (operatingSystem == "Windows")
            {
                sb.Append("Write-Host \"");
            }
            else if (operatingSystem == "Linux/Mac")
            {
                sb.Append("printf \"");
            }
            else
            {
                sb.Append("// Unsupported OS");
            }

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
                        int r = (int)Math.Floor(Math.Clamp(pixelTop.R / 255.0 * 5, 0, 5));
                        int g = (int)Math.Floor(Math.Clamp(pixelTop.G / 255.0 * 5, 0, 5));
                        int b = (int)Math.Floor(Math.Clamp(pixelTop.B / 255.0 * 5, 0, 5));
                        int pixelTopSmall = 16 + 36 * r + 6 * g + b;

                        r = (int)Math.Floor(Math.Clamp(pixelBottom.R / 255.0 * 5, 0, 5));
                        g = (int)Math.Floor(Math.Clamp(pixelBottom.G / 255.0 * 5, 0, 5));
                        b = (int)Math.Floor(Math.Clamp(pixelBottom.B / 255.0 * 5, 0, 5));
                        int pixelBottomSmall = 16 + 36 * r + 6 * g + b;

                        fg = $"{escape}[38;5;{pixelTopSmall}m";
                        bg = $"{escape}[48;5;{pixelBottomSmall}m";
                    }
                    else
                    {
                        fg = $"{escape}[38;2;{pixelTop.R};{pixelTop.G};{pixelTop.B}m";
                        bg = $"{escape}[48;2;{pixelBottom.R};{pixelBottom.G};{pixelBottom.B}m";
                    }

                    sb.Append($"{fg}{bg}▀");
                }
                sb.Append($"{escape}[0m{newLine}");
            }
            sb.Append($"{escape}[0m");
            sb.Append("\"");
            return sb.ToString();
        }

        public static string SVG(Stream imageStream, string characters, int scale, bool invert, string font, string background, bool useBackgroundColor)
        {
            var sb = new StringBuilder();

            using Image<Rgba32> image = Image.Load<Rgba32>(imageStream);

            var memoryGroup = image.GetPixelMemoryGroup();

            var pixelMemory = memoryGroup[0];
            var pixels = pixelMemory.Span;

            int width = image.Width;
            int height = image.Height;

            sb.AppendLine($"<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 {width / scale * 16} {height / scale * 32}\" width=\"100%\" height=\"auto\" font-family=\"{font}\" font-size=\"32\" preserveAspectRatio=\"xMinYMin meet\">");

            string bg = useBackgroundColor ? background : "transparent";
            sb.AppendLine($"<rect width=\"100%\" height=\"100%\" fill=\"{bg}\" />");

            int ySVG = 24;
            for (int y = 0; y < height; y += scale)
            {
                sb.AppendLine($"<text y=\"{ySVG}\">");
                ySVG += 32;
                int xSVG = 0;
                for (int x = 0; x < width; x += scale)
                {
                    int index = y * width + x;

                    Rgba32 pixel = pixels[index];

                    int brightness = (pixel.R + pixel.G + pixel.B) / 3;
                    if (invert)
                        brightness = 255 - brightness;

                    int charIndex = brightness * (characters.Length - 1) / 255;

                    string hex = $"{pixel.R:X2}{pixel.G:X2}{pixel.B:X2}";
                    sb.AppendLine($"<tspan x=\"{xSVG}\" fill=\"#{hex}\">{characters[charIndex]}</tspan>");
                    xSVG += 16;
                }
                sb.AppendLine("</text>");
            }
            sb.AppendLine("</svg>");
            return sb.ToString();
        }

        public static string RTF(Stream imageStream, string characters, int scale, bool invert, string font, string background, bool useBackgroundColor)
        {
            var head = new StringBuilder();
            var text = new StringBuilder();
            var definedColors = new List<Rgba32>();

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

                    int brightness = (pixel.R + pixel.G + pixel.B) / 3;
                    if (invert)
                        brightness = 255 - brightness;

                    int charIndex = brightness * (characters.Length - 1) / 255;

                    if (!definedColors.Contains(pixel))
                    {
                        definedColors.Add(pixel);
                    }
                    int indexOfColor = definedColors.IndexOf(pixel);
                    text.Append($"\\cf{indexOfColor + 1} {characters[charIndex]}");
                }
                text.AppendLine("\\line");
            }

            head.AppendLine("{\\rtf1\\ansi\\deff0");
            head.Append("{\\fonttbl{\\f0\\fmodern\\fcharset0 Consolas;}}\n");
            head.Append("{\\colortbl ;");
            foreach (var item in definedColors)
            {
                head.Append($"\\red{item.R}\\green{item.G}\\blue{item.B};");
            }
            Rgba32 bg = new Rgba32();
            if (useBackgroundColor)
            {
                var (r, g, b) = HexToRGB("#FFAA33");
                bg = new Rgba32(r, g, b);
                if (!definedColors.Contains(bg))
                {
                    definedColors.Add(bg);
                }

            }

            head.Append("}\n");

            if (useBackgroundColor)
            {
                int indexOfBg = definedColors.IndexOf(bg);
                head.AppendLine($"\\highlight{indexOfBg + 1} ");
            }

            head.Append("\\f0\\sl100\\slmult1\n");
            head.Append(text);

            head.Append("}");
            return head.ToString();
        }

        public static string EMOJI(Stream imageStream, string characters, int scale, bool invert)
        {
            var text = new StringBuilder();

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

                    if (invert)
                    {
                        pixel = new Rgba32(
                            (byte)(255 - pixel.R),
                            (byte)(255 - pixel.G),
                            (byte)(255 - pixel.B),
                            pixel.A
                        );
                    }

                    text.Append(ClosestEmoji(pixel.R, pixel.G, pixel.B));
                }
                text.AppendLine("\n");
            }

            return text.ToString();
        }

        public static string TXT(Stream imageStream, string characters, int scale, bool invert)
        {
            var text = new StringBuilder();

            using Image<Rgba32> image = Image.Load<Rgba32>(imageStream);

            var memoryGroup = image.GetPixelMemoryGroup();

            var pixelMemory = memoryGroup[0];
            var pixels = pixelMemory.Span;

            int width = image.Width;
            int height = image.Height;

            for (int y = 0; y < height; y += (scale*2))
            {
                for (int x = 0; x < width;)
                {
                    int initialX = x;
                    int index = y * width;

                    Rgba32 pixel = pixels[index + x];

                    x += scale;
                    while (x < width && pixel.Equals(pixels[index + x]))
                        x += scale;

                    int brightness = (pixel.R + pixel.G + pixel.B) / 3;
                    if (invert)
                        brightness = 255 - brightness;

                    int charIndex = brightness * (characters.Length - 1) / 255;

                    int count = (x - initialX) / scale;
                    count *= 2;

                    text.Append(count > 1 ? new string(characters[charIndex], count) : characters[charIndex]);
                }
                text.Append("\n");
            }

            // Widths [4.4453 .. 4.4453]:  !,./:;I[\]t
            // Widths [5.3281 .. 5.3438]: ()-`r{}
            // Widths [8.0000 .. 8.0000]: Jcksvxyz
            // Widths [8.8984 .. 8.8984]: #$023456789?Labdeghnopqu_                 //toto pre compatibility mode (ked to nie je monospace font, napr arial)
            // Widths [9.3438 .. 9.3438]: +<=>~
            // Widths [9.7734 .. 9.7734]: FTZ
            // Widths [10.6719 .. 10.6719]: &ABEKPSVXY
            // Widths [11.5547 .. 11.5547]: CDHNRUw

            return text.ToString();
        }

        private static string ClosestEmoji(int r, int g, int b)
        {
            var emojiPalette = new Dictionary<string, (int R, int G, int B)>
            {
                ["\U0001F7E5"] = (209, 34, 41),         // Red
                ["\U0001F7E7"] = (255, 139, 0),         // Orange
                ["\U0001F7E8"] = (255, 205, 0),         // Yellow
                ["\U0001F7E9"] = (0, 135, 62),          // Green
                ["\U0001F7EB"] = (0, 102, 204),         // Blue
                ["\U0001F7EA"] = (128, 0, 128),         // Purple
                ["\u2B1C"]     = (255, 255, 255),       // White
                ["\u2B1B"]     = (0, 0, 0)              // Black
            };

            return emojiPalette
                .OrderBy(e => Math.Pow(r - e.Value.R, 2) + Math.Pow(g - e.Value.G, 2) + Math.Pow(b - e.Value.B, 2))
                .First().Key;
        }

        private static (int R, int G, int B) HexToRGB(string hex)
        {
            if (hex.StartsWith("#"))
                hex = hex.Substring(1);

            if (hex.Length != 6)
                throw new ArgumentException("Hex color must be 6 characters (e.g. #FFAABB)");

            int r = System.Convert.ToInt32(hex.Substring(0, 2), 16);
            int g = System.Convert.ToInt32(hex.Substring(2, 2), 16);
            int b = System.Convert.ToInt32(hex.Substring(4, 2), 16);

            return (r, g, b);
        }
    }
}
