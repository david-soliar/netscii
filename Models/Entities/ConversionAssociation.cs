namespace netscii.Models.Entities
{

    public class ConversionAssociation
    {
        public int ConversionActivityId { get; set; }
        public ConversionActivity ConversionActivity { get; set; } = null!;

        public int ConversionParametersId { get; set; }
        public ConversionParameters ConversionParameters { get; set; } = null!;
    }
}
