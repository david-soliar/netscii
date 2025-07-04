﻿namespace netscii.Models
{
    public class ConversionViewModel
    {
        public IFormFile Image { get; set; } = null!;
        public string Characters { get; set; } = string.Empty;
        public int Scale { get; set; } = 8;
        public bool Invert { get; set; } = false;
        public string Font { get; set; } = string.Empty;
        public List<string> Fonts { get; set; } = new List<string>();
        public string Background { get; set; } = "#FFFFFF";
        public bool UseBackgroundColor { get; set; } = false;
        public bool CreateFullDocument { get; set; } = true;
        public string OperatingSystem { get; set; } = string.Empty;
        public List<string> OperatingSystems { get; set; } = new List<string>();
        public bool UseSmallPalette { get; set; } = false;
        public string Controller { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
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
