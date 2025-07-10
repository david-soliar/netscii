using netscii.Utils.ImageConverters.Exceptions;
using netscii.Utils.ImageConverters.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Text;

namespace netscii.Utils.ImageConverters.Converters
{
    public static class LatexConverter
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
                options.Font = "inconsolata";


            image.Mutate(x => x.Resize(image.Width / options.Scale, image.Height / options.Scale));

            var result = new ConverterResult { Width = image.Width, Height = image.Height };

            var document = new StringBuilder();
            var tex = new StringBuilder();

            document.AppendLine("\\documentclass{article}");
            document.AppendLine("\\usepackage{xcolor}");
            document.AppendLine($"\\usepackage{{{options.Font}}}");
            document.AppendLine("\\linespread{0}");

            if (options.UseBackgroundColor && !string.IsNullOrWhiteSpace(options.Background))
            {
                document.AppendLine("\\definecolor{mybg}{rgb}{0.1,0.2,0.3}");
                document.AppendLine("\\pagecolor{mybg}");
            }

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

                    int initialX = x;
                    int count = 1;

                    while (i + count < pixels.Length && pixels[i + count].Equals(pixel) && (x + count) < image.Width)
                    {
                        count++;
                    }

                    int charIndex = ConverterHelpers.GetCharIndex(pixel, options.Characters.Length);

                    tex.AppendFormat("{{\\color[rgb]{{{0},{1},{2}}}{3}}}",
                        pixel.R / 255.0, pixel.G / 255.0, pixel.B / 255.0,
                        new string(options.Characters[charIndex], count));

                    i += count;
                    x += count;

                    if (x >= image.Width)
                    {
                        x = 0;
                        tex.AppendLine("\\newline");
                    }
                }
            }

            double charWidthCm = 0.214;
            double charHeightCm = 0.28;
            double paperWidthCm = charWidthCm * image.Width;
            double paperHeightCm = charHeightCm * image.Height;

            document.AppendLine($"\\usepackage[paperwidth={paperWidthCm}cm, paperheight={paperHeightCm}cm, margin=0pt]{{geometry}}");

            document.AppendLine("\n\\begin{document}\n");
            document.AppendLine("\\vbox to \\textheight{");
            document.AppendLine("\t\\vfill");
            document.AppendLine("\t\\begin{center}");
            document.AppendLine("\t\t{\\ttfamily");
            document.AppendLine("\t\t\t\\setlength{\\baselineskip}{0pt}");

            document.Append(tex);

            document.AppendLine("\t\t}");
            document.AppendLine("\t\\end{center}");
            document.AppendLine("\t\\vfill");
            document.AppendLine("}");
            document.AppendLine("\\end{document}").AppendLine();

            result.Content = document.ToString();
            return result;
        }
    }
}
