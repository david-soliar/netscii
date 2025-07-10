using System.Text.Json.Serialization;

namespace netscii.Models.Dto
{
    public class JsonConversionDto
    {
        public string Image { get; set; } = string.Empty;
        public string Characters { get; set; } = string.Empty;
        public int Scale { get; set; } = 8;
        public bool Invert { get; set; } = false;
        public string Font { get; set; } = string.Empty;
        public string Background { get; set; } = string.Empty;

        [JsonIgnore]
        public bool UseBackgroundColor { get; set; } = false;

        public string Platform { get; set; } = string.Empty;
        public bool UseSmallPalette { get; set; } = false;
        public int Period { get; set; } = 24;
    }
}
