namespace netscii.Utils.ImageConverters.Models
{
    public class ConverterResult
    {
        public int Width { get; set; } = 0;
        public int Height { get; set; } = 0;
        public long ProcessingTimeMs { get; set; } = 0;
        public int OutputLengthBytes { get; set; } = 0;
        public string Format { get; set; } = string.Empty;
        public string MimeType { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
