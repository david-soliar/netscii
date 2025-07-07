using netscii.Utils.ImageConverters.Exceptions;
using netscii.Utils.ImageConverters.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using System.Text;
using System.Text.Json.Nodes;


namespace netscii.Utils.ImageConverters.Converters
{
    public static class HtmlConverter
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
                options.Font = "monospace";


            var css = new StringBuilder();
            var html = new StringBuilder();

            var memoryGroup = image.GetPixelMemoryGroup();

            var pixelMemory = memoryGroup[0];
            var pixels = pixelMemory.Span;

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
                }
                html.Append("\n");
            }

            css.Append("</style>");
            html.Append("</pre>");

            result.Content = css.Append(html).ToString();
            return result;
        }
    }
}
