namespace netscii.Models.Entities
{
    public class SuggestionCategory
    {
        public int Id { get; set; }
        public string CategoryName { get; set; } = null!;

        public ICollection<SuggestionCategoryAssociation> SuggestionAssociations { get; set; } = new List<SuggestionCategoryAssociation>();
    }
}
