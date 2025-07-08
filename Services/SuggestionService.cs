using netscii.Models.Dto;
using netscii.Models.Entities;
using netscii.Repositories;

namespace netscii.Services
{
    public class SuggestionService
    {
        private readonly SuggestionRepository _repository;

        public SuggestionService(SuggestionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<string>> GetCategoryNamesAsync()
        {
            return await _repository.GetCategoryNamesAsync();
        }

        public async Task<List<SuggestionDisplayDto>> GetAllSuggestionsAsync()
        {
            var suggestions = await _repository.GetAllSuggestionsAsync();

            return suggestions.Select(s => new SuggestionDisplayDto
            {
                Text = s.Text,
                CreatedAt = s.CreatedAt,
                Categories = s.SuggestionCategoryAssociations
                    .Select(a => a.SuggestionCategory.CategoryName)
                    .ToList()
            }).ToList();
        }

        public async Task<List<SuggestionDisplayDto>> GetSuggestionsByCategoriesAsync(List<string> categories)
        {
            var suggestions = await _repository.GetSuggestionsByCategoryAsync(categories);

            return suggestions.Select(s => new SuggestionDisplayDto
            {
                Text = s.Text,
                CreatedAt = s.CreatedAt,
                Categories = s.SuggestionCategoryAssociations
                    .Select(a => a.SuggestionCategory.CategoryName)
                    .ToList()
            }).ToList();
        }

        public async Task AddSuggestionAsync(string text, List<string> categories)
        {
            await _repository.AddSuggestionAsync(text, categories);
            return;
        }
    }
}
