using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;
using System.Text;


namespace netscii.Utils
{
    public static class HTMLConverter
    {
        public static string Convert(System.IO.Stream imageStream, string characters, int scale, bool invert)
        {
            using Image<Rgba32> image = Image.Load<Rgba32>(imageStream);
            var sb = new StringBuilder();

            sb.Append("<pre style='line-height: 80%; font-family: monospace;'>");

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

                    sb.Append("<span style='color: rgb(");
                    sb.Append(pixel.R.ToString()); sb.Append(",");
                    sb.Append(pixel.G.ToString()); sb.Append(",");
                    sb.Append(pixel.B.ToString());
                    sb.Append(")'>");
                    sb.Append(characters[charIndex]);
                    sb.Append("</span>");
                }
                sb.Append("<br>");
            }

            sb.Append("</pre>");

            return sb.ToString();
        }
    }

    public static class MDConverter
    {
        public static string Convert(System.IO.Stream imageStream, string characters, int scale, bool invert)
        {
            // Your MD conversion logic here
            return "{\\rtf1\\ansi ...}";
        }
    }

    public static class ANSIConverter
    {
        public static string Convert(System.IO.Stream imageStream, string characters, int scale, bool invert)
        {
            // Your ANSI conversion logic here
            return "{\\rtf1\\ansi ...}";
        }
    }

    public static class LATEXConverter
    {
        public static string Convert(System.IO.Stream imageStream, string characters, int scale, bool invert)
        {
            // Your LATEX conversion logic here
            return "{\\rtf1\\ansi ...}";
        }
    }

    public static class RTFConverter
    {
        public static string Convert(System.IO.Stream imageStream, string characters, int scale, bool invert)
        {
            // Your RTF conversion logic here
            return "{\\rtf1\\ansi ...}";
        }
    }
}
