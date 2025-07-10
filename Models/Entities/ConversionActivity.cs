using System.Text.Json.Serialization;

namespace netscii.Models.Entities
{
    public class ConversionActivity
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Format { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public long ProcessingTimeMs { get; set; }
        public int OutputLengthBytes { get; set; }

        [JsonIgnore]
        public int ConversionParametersId { get; set; }

        [JsonIgnore]
        public ConversionParameters ConversionParameters { get; set; } = null!;
    }
}
