namespace netscii.Models.ViewModels
{
    public class ErrorViewModel
    {
        public int Code { get; set; } = 500;
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
