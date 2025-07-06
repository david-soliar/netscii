using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System.Text;
using netscii.Utils.ImageConverters.Models;
using SixLabors.ImageSharp.Advanced;

namespace netscii.Utils.ImageConverters.Converters
{
    public class EmojiConverter
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


            var text = new StringBuilder();

            var memoryGroup = image.GetPixelMemoryGroup();

            var pixelMemory = memoryGroup[0];
            var pixels = pixelMemory.Span;

            for (int y = 0; y < height; y += options.Scale)
            {
                for (int x = 0; x < width; x += options.Scale)
                {
                    int index = y * width + x;

                    Rgba32 pixel = pixels[index];

                    if (options.Invert)
                        pixel = ConverterHelpers.InvertPixel(pixel);

                    text.Append(ConverterHelpers.ClosestEmoji(pixel));
                }
                text.AppendLine("\n");
            }

            return text.ToString();
        }
    }
}
