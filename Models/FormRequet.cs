namespace netscii.Models
{
    public class FormRequest
    {
        public IFormFile Image { get; set; } = null!;
        public string Characters { get; set; } = string.Empty;
        public int Scale { get; set; } = 16;
        public bool Invert { get; set; } = false;
        public string Font { get; set; } = string.Empty;
        public string Background { get; set; } = "#FFFFFF";
        public bool UseBackgroundColor { get; set; } = false;
        public bool CreateFullDocument { get; set; } = true;
        public string OperatingSystem { get; set; } = "Linux/Mac";
        public bool UseSmallPalette { get; set; } = false;

        public string Status { get; set; } = string.Empty;

        public Stream GetImageStream() => Image.OpenReadStream();

        public bool IsInvalid()
        {
            if (Image == null || Image.Length == 0)
            {
                Status = "Image file is required.";
                return true;
            }
            return false;
        }
    }
}
