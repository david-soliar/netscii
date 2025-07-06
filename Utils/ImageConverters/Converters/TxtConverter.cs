using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System.Text;
using netscii.Utils.ImageConverters.Models;
using SixLabors.ImageSharp.Advanced;
using netscii.Utils.ImageConverters.Exceptions;


namespace netscii.Utils.ImageConverters.Converters
{
    public static class TxtConverter
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


            var text = new StringBuilder();

            var memoryGroup = image.GetPixelMemoryGroup();

            var pixelMemory = memoryGroup[0];
            var pixels = pixelMemory.Span;

            for (int y = 0; y < height; y += (options.Scale * 2))
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

                    int count = (x - initialX) / options.Scale * 2;

                    int charIndex = ConverterHelpers.GetCharIndex(pixel, options.Characters.Length);

                    text.Append(count > 1 ? new string(options.Characters[charIndex], count) : options.Characters[charIndex]);
                }
                text.Append("\n");
            }

            result.Content = text.ToString();
            return result;
        }
    }
}
