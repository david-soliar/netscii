using netscii.Models.Entities;

namespace netscii.Models.Dto
{
    public class LogDto
    {
        public ConversionActivity? Activity { get; set; }
        public ConversionParameters? Parameters { get; set; }
    }
}
