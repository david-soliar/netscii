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

        public async Task<List<Suggestion>> GetAllSuggestionsAsync()
        {
            return await _repository.GetAllSuggestionsAsync();
        }

        public async Task<List<Suggestion>> GetSuggestionsByCategoriesAsync(List<string> categories)
        {

            return await _repository.GetSuggestionsByCategoryAsync(categories);
        }

        public async Task AddSuggestionAsync(string text, List<string> categories)
        {
            await _repository.AddSuggestionAsync(text, categories);
            return;
        }
    }
}
