namespace netscii.Models.ViewModels
{
    public class ConversionViewModel
    {
        public IFormFile Image { get; set; } = null!;
        public string Characters { get; set; } = string.Empty;
        public int Scale { get; set; } = 8;
        public bool Invert { get; set; } = false;
        public string Font { get; set; } = string.Empty;
        public List<string> Fonts { get; set; } = new List<string>();
        public string Background { get; set; } = string.Empty;
        public bool UseBackgroundColor { get; set; } = false;
        public string Platform { get; set; } = string.Empty;
        public List<string> Platforms { get; set; } = new List<string>();
        public bool UseSmallPalette { get; set; } = false;
        public string Format { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        public Stream GetImageStream() => Image.OpenReadStream();
    }
}
