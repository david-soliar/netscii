namespace netscii.Utils.ImageConverters.Models
{
    public class ConverterOptions
    {
        public Stream Image { get; set; } = null!;
        public string Characters { get; set; } = string.Empty;
        public int Scale { get; set; } = 8;
        public bool Invert { get; set; } = false;
        public string Font { get; set; } = string.Empty;
        public string Background { get; set; } = "#FFFFFF";
        public bool UseBackgroundColor { get; set; } = false;
        public string Platform { get; set; } = string.Empty;
        public bool UseSmallPalette { get; set; } = false;
    }
}
