namespace netscii.Models.ViewModels
{
    public class JsonConversionViewModel
    {
        public string Image { get; set; } = string.Empty;
        public string Characters { get; set; } = string.Empty;
        public int Scale { get; set; } = 8;
        public bool Invert { get; set; } = false;
        public string Font { get; set; } = string.Empty;
        public string Background { get; set; } = string.Empty;
        public bool UseBackgroundColor { get; set; } = false;
        public bool CreateFullDocument { get; set; } = true;
        public string OperatingSystem { get; set; } = string.Empty;
        public bool UseSmallPalette { get; set; } = false;
    }
}
