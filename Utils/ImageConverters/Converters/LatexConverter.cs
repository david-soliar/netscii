using netscii.Utils.ImageConverters.Exceptions;
using netscii.Utils.ImageConverters.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using System.Text;


namespace netscii.Utils.ImageConverters.Converters
{
    public static class LatexConverter
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

            if (options.UseBackgroundColor && !string.IsNullOrWhiteSpace(options.Background))
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

                    tex.AppendFormat("\n\t\t\t{{\\color[rgb]{{{0},{1},{2}}} {3}}}",
                        pixel.R / 255.0, pixel.G / 255.0, pixel.B / 255.0,
                        new string(options.Characters[charIndex], count));
                }
                tex.AppendLine("\n\t\t\t\\newline");
            }

            if (options.CreateFullDocument)
            {
                double charWidthCm = 0.2;
                double charHeightCm = 0.28;
                double paperWidthCm = charWidthCm * width;
                double paperHeightCm = charHeightCm * height;

                document.AppendLine($"\\usepackage[paperwidth={paperWidthCm}cm, paperheight={paperHeightCm}cm, margin=0pt]{{geometry}}");

                document.AppendLine("\n\\begin{document}\n");
                document.AppendLine("\\vbox to \\textheight{");
                document.AppendLine("\t\\vfill");
                document.AppendLine("\t\\begin{center}");
                document.AppendLine("\t\t{\\ttfamily");
                document.AppendLine("\t\t\t\\setlength{\\baselineskip}{0pt}");
            }

            document.Append(tex);

            if (options.CreateFullDocument)
            {
                document.AppendLine("\t\t}");
                document.AppendLine("\t\\end{center}");
                document.AppendLine("\t\\vfill");
                document.AppendLine("}");
                document.AppendLine("\\end{document}").AppendLine();
            }

            result.Content = document.ToString();
            return result;
        }
    }
}
