using netscii.Utils.ImageConverters.Models;
using netscii.Utils.ImageConverters.Converters;
using netscii.Utils.ImageConverters.Exceptions;
using System.Diagnostics;
using System.Text;

namespace netscii.Utils.ImageConverters
{
    public static class ConverterDispatcher
    {
        public static readonly Dictionary<string, Func<Stream, ConverterOptions, ConverterResult>> Converters =
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

        public static readonly Dictionary<string, string> MimeTypes = 
            new(StringComparer.OrdinalIgnoreCase)
            {
                { "html", "text/html" },
                { "txt", "text/plain" },
                { "latex", "text/x-latex" },
                { "rtf", "application/rtf" },
                { "svg", "image/svg+xml" },
                { "ansi", "text/plain" },
                { "emoji", "text/plain" }
            };

        public static readonly IReadOnlyList<string> SupportedPlatforms =
            new List<string>
            {
                "Windows Console",
                "Unix-like Shell"
            }.AsReadOnly();

        public static ConverterResult Convert(string format, Stream stream, ConverterOptions options)
        {
            if (!Converters.TryGetValue(format, out var converter))
                throw new ConverterException(ConverterErrorCode.UnsupportedFormat);

            var stopwatch = Stopwatch.StartNew();

            ConverterResult result = converter(stream, options);

            stopwatch.Stop();

            result.ProcessingTimeMs = stopwatch.ElapsedMilliseconds;
            result.OutputLengthBytes = Encoding.UTF8.GetByteCount(result.Content);
            result.MimeType = MimeTypes.GetValueOrDefault(format, "application/x-unknown");
            result.Format = format;

            return result;
        }
    }
}
