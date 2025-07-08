using netscii.Models.Entities;

namespace netscii.Models.ViewModels
{
    public class SuggestionViewModel
    {
        public List<string> Categories { get; set; } = new List<string>();
        public List<string> SelectedCategories { get; set; } = new List<string>();
        public List<Suggestion>? Suggestions { get; set; } = null;
        public string Text { get; set; } = string.Empty;
    }
}
