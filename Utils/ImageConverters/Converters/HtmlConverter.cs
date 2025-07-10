using netscii.Utils.ImageConverters.Exceptions;
using netscii.Utils.ImageConverters.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Text;

namespace netscii.Utils.ImageConverters.Converters
{
    public static class HtmlConverter
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

            var css = new StringBuilder();
            var html = new StringBuilder();

            var definedColors = new HashSet<string>();

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
            options.Font,
                options.UseBackgroundColor && !string.IsNullOrWhiteSpace(options.Background) ? options.Background : "transparent");


            html.Append("\n<pre id=\"netscii-html-result\">\n");

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
                    string hex = $"{pixel.R:X2}{pixel.G:X2}{pixel.B:X2}";
                    string className = $"c{hex}";

                    if (!definedColors.Contains(className))
                    {
                        css.AppendFormat(".{0} {{\n\tcolor: #{1};\n}}\n", className, hex);
                        definedColors.Add(className);
                    }

                    html.AppendFormat("<span class='{0}'>", className);
                    html.Append(count > 1 ? new string(options.Characters[charIndex], count) : options.Characters[charIndex]);
                    html.Append("</span>");

                    i += count;
                    x += count;

                    if (x >= image.Width)
                    {
                        x = 0;
                        html.Append("\n");
                    }
                }
            }


            css.Append("</style>");
            html.Append("</pre>");

            result.Content = css.Append(html).ToString();
            return result;
        }
    }
}
