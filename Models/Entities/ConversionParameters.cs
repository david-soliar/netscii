using System.Text.Json.Serialization;

namespace netscii.Models.Entities
{
    public class ConversionParameters
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Characters { get; set; } = null!;
        public string Font { get; set; } = null!;
        public string Background { get; set; } = null!;
        public int Scale { get; set; }
        public bool Invert { get; set; }
        public string Platform { get; set; } = null!;

        [JsonIgnore]
        public ICollection<ConversionActivity> Activities { get; set; } = new List<ConversionActivity>();
    }
}
