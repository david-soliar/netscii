using netscii.Utils.ImageConverters.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Text;

namespace netscii.Utils.ImageConverters.Converters
{
    public static class LatexConverter
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
                options.Font = "inconsolata";


            var document = new StringBuilder();
            var tex = new StringBuilder();

            if (options.CreateFullDocument)
            {
                document.AppendLine("\\documentclass{article}");
                document.AppendLine("\\usepackage{xcolor}");
                document.AppendLine($"\\usepackage{{{options.Font}}}");
                document.AppendLine("\\linespread{0}");
            }

            if (options.UseBackgroundColor)
            {
                document.AppendLine("\\definecolor{mybg}{rgb}{0.1,0.2,0.3}");
                document.AppendLine("\\pagecolor{mybg}");
            }

            var memoryGroup = image.GetPixelMemoryGroup();

            var pixelMemory = memoryGroup[0];
            var pixels = pixelMemory.Span;

            for (int y = 0; y < height; y += options.Scale)
            {
                for (int x = 0; x < width;)
                {
                    int initialX = x;
                    int index = y * width;

                    Rgba32 pixel = pixels[index + x];
                    if (options.Invert)
                        pixel = ConverterHelpers.InvertPixel(pixel);

                    x += options.Scale;
                    while (x < width && pixel.Equals(pixels[index + x]))
                        x += options.Scale;

                    int count = (x - initialX) / options.Scale;

                    int charIndex = ConverterHelpers.GetCharIndex(pixel, options.Characters.Length);

                    tex.AppendFormat("{{\\color[rgb]{{{0},{1},{2}}} {3}}}",
                        pixel.R / 255.0, pixel.G / 255.0, pixel.B / 255.0,
                        new string(options.Characters[charIndex], count));
                }
                tex.AppendLine(@"\newline").AppendLine();
            }

            if (options.CreateFullDocument)
            {
                double charWidthCm = 0.2;
                double charHeightCm = 0.28;
                double paperWidthCm = charWidthCm * width;
                double paperHeightCm = charHeightCm * height;

                document.AppendLine($"\\usepackage[paperwidth={paperWidthCm}cm, paperheight={paperHeightCm}cm, margin=0pt]{{geometry}}");

                document.AppendLine("\\begin{document}");
                document.AppendLine("\\vbox to \\textheight{");
                document.AppendLine("\\vfill");
                document.AppendLine("\\begin{center}");
                document.AppendLine("{\\ttfamily");
                document.AppendLine("\\setlength{\\baselineskip}{0pt}");
            }

            document.Append(tex);

            if (options.CreateFullDocument)
            {
                document.AppendLine("}");
                document.AppendLine("\\end{center}").AppendLine();
                document.AppendLine("\\vfill");
                document.AppendLine("}");
                document.AppendLine("\\end{document}").AppendLine();
            }

            return document.ToString();
        }
    }
}
