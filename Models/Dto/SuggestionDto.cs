namespace netscii.Models.Dto
{
    public class SuggestionDisplayDto
    {
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<string> Categories { get; set; } = new();
    }
}
