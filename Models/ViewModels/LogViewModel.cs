using netscii.Models.Dto;

namespace netscii.Models.ViewModels
{
    public class LogViewModel
    {
        public string Period { get; set; } = "24h";

        public List<LogDto> Logs { get; set; } = new List<LogDto>();
    }
}
