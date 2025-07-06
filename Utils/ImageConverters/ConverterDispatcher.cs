using netscii.Utils.ImageConverters.Models;
using netscii.Utils.ImageConverters.Converters;

namespace netscii.Utils.ImageConverters
{
    public static class ConverterDispatcher
    {
        private static readonly Dictionary<string, Func<Stream, ConverterOptions, string>> _map =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["html"] = HtmlConverter.Convert,
                ["txt"] = TxtConverter.Convert,
                ["latex"] = LatexConverter.Convert,
                ["rtf"] = RtfConverter.Convert,
                ["svg"] = SvgConverter.Convert,
                ["ansi"] = AnsiConverter.Convert,
                ["emoji"] = EmojiConverter.Convert
            };

        public static string Convert(string format, Stream stream, ConverterOptions options)
        {
            if (!_map.TryGetValue(format, out var converter))
                throw new NotSupportedException($"Unsupported format: {format}");

            return converter(stream, options);
        }
    }
}
