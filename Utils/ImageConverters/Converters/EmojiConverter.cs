using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System.Text;
using netscii.Utils.ImageConverters.Models;
using SixLabors.ImageSharp.Advanced;
using netscii.Utils.ImageConverters.Exceptions;

namespace netscii.Utils.ImageConverters.Converters
{
    public class EmojiConverter
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

            result.Content = text.ToString();
            return result;
        }
    }
}
