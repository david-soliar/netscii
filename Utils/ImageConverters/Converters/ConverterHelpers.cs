using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using netscii.Utils.ImageConverters.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.IO.Pipelines;
using System.Text;


namespace netscii.Utils.ImageConverters.Converters
{
    public static class ConverterHelpers
    {
        private static readonly Dictionary<string, (int R, int G, int B)> EmojiPalette = new()
        {
            ["\U0001F7E5"] = (209, 34, 41),         // Red
            ["\U0001F7E7"] = (255, 139, 0),         // Orange
            ["\U0001F7E8"] = (255, 205, 0),         // Yellow
            ["\U0001F7E9"] = (0, 135, 62),          // Green
            ["\U0001F7EB"] = (0, 102, 204),         // Blue
            ["\U0001F7EA"] = (128, 0, 128),         // Purple
            ["\u2B1C"] = (255, 255, 255),           // White
            ["\u2B1B"] = (0, 0, 0)                  // Black
        };

        public static int[] Convert(Stream imageStream, int scale, Action<Rgba32, int> processPixel, Action<int> processRow)
        {
            using Image<Rgba32> image = Image.Load<Rgba32>(imageStream);

            var memoryGroup = image.GetPixelMemoryGroup();

            var pixelMemory = memoryGroup[0];
            var pixels = pixelMemory.Span;

            int width = image.Width;
            int height = image.Height;

            for (int y = 0; y < height; y += scale)
            {
                for (int x = 0; x < width;)
                {
                    int initialX = x;
                    int index = y * width;

                    Rgba32 pixel = pixels[index + x];

                    x += scale;
                    while (x < width && pixel.Equals(pixels[index + x]))
                        x += scale;

                    processPixel(pixel, (x - initialX) / scale);
                }
                processRow(y);
            }
            return new int[] { width / scale, height / scale };
        }

        public static int GetCharIndex(Rgba32 pixel, int length)
        {
            int brightness = (int)(((pixel.R + pixel.G + pixel.B) / 3f) * (pixel.A / 255f));

            return brightness * (length - 1) / 255;
        }

        public static string ClosestEmoji(Rgba32 pixel)
        {
            return EmojiPalette
                .OrderBy(e => Math.Pow(pixel.R - e.Value.R, 2) + Math.Pow(pixel.G - e.Value.G, 2) + Math.Pow(pixel.B - e.Value.B, 2))
                .First().Key;
        }

        public static string RGBToHex(int r, int g, int b)
        {
            return $"#{r:X2}{g:X2}{b:X2}";
        }

        public static (int R, int G, int B) HexToRGB(string hex)
        {
            if (hex.StartsWith("#"))
                hex = hex.Substring(1);

            if (hex.Length != 6)
                throw new ArgumentException("Hex color must be 6 characters (e.g. #FFAABB)");

            int r = System.Convert.ToInt32(hex.Substring(0, 2), 16);
            int g = System.Convert.ToInt32(hex.Substring(2, 2), 16);
            int b = System.Convert.ToInt32(hex.Substring(4, 2), 16);

            return (r, g, b);
        }

        public static int To256ColorCode(Rgba32 pixel)
        {
            int r = (int) Math.Floor(Math.Clamp(pixel.R / 255.0 * 5, 0, 5));
            int g = (int) Math.Floor(Math.Clamp(pixel.G / 255.0 * 5, 0, 5));
            int b = (int) Math.Floor(Math.Clamp(pixel.B / 255.0 * 5, 0, 5));
            return (16 + 36 * r + 6 * g + b);
        }

        public static Rgba32 InvertPixel(Rgba32 pixel)
        {
            return new Rgba32(
                (byte)(255 - pixel.R),
                (byte)(255 - pixel.G),
                (byte)(255 - pixel.B),
                pixel.A);
        }
    }
}
