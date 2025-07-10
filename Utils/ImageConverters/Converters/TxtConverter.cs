using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System.Text;
using netscii.Utils.ImageConverters.Models;
using SixLabors.ImageSharp.Advanced;
using netscii.Utils.ImageConverters.Exceptions;
using SixLabors.ImageSharp.Processing;

namespace netscii.Utils.ImageConverters.Converters
{
    public static class TxtConverter
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


            image.Mutate(x => x.Resize(image.Width / options.Scale, image.Height / (options.Scale * 2)));

            var result = new ConverterResult { Width = image.Width, Height = image.Height };

            var text = new StringBuilder();

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
                    text.Append(count > 1 ? new string(options.Characters[charIndex], count) : options.Characters[charIndex]);

                    i += count;
                    x += count;

                    if (x >= image.Width)
                    {
                        x = 0;
                        text.Append("\n");
                    }
                }
            }

            result.Content = text.ToString();
            return result;
        }
    }
}
