namespace netscii.Models.Entities
{
    public class SuggestionCategoryAssociation
    {
        public int SuggestionId { get; set; }
        public Suggestion Suggestion { get; set; } = null!;

        public int SuggestionCategoryId { get; set; }
        public SuggestionCategory SuggestionCategory { get; set; } = null!;
    }
}
