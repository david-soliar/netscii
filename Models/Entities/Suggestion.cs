namespace netscii.Models.Entities
{
    public class Suggestion
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<SuggestionCategoryAssociation> SuggestionCategoryAssociations { get; set; } = new List<SuggestionCategoryAssociation>();
    }
}
