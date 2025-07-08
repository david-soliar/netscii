namespace netscii.Models.Entities
{
    public class ConversionActivity
    {
        public int Id { get; set; }
        public string Format { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public long ProcessingTimeMs { get; set; }
        public int OutputLengthBytes { get; set; }

        public int ConversionParametersId { get; set; }
        public ConversionParameters ConversionParameters { get; set; } = null!;
    }
}
