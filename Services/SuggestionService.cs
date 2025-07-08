using Microsoft.Extensions.Caching.Memory;
using netscii.Constants;
using netscii.Models.Dto;
using netscii.Repositories;

namespace netscii.Services
{
    public class SuggestionService
    {
        private readonly SuggestionRepository _repository;
        private readonly IMemoryCache _cache;

        public SuggestionService(SuggestionRepository repository, IMemoryCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<List<string>> GetCategoryNamesAsync()
        {
            var result = await _cache.GetOrCreateAsync<List<string>>(CacheKeys.CategoryNames, async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                    return await _repository.GetCategoryNamesAsync();
                });

            return result!;
        }

        public async Task<List<SuggestionDisplayDto>> GetSuggestionsByCategoriesAsync(List<string> categories)
        {
            var allSuggestions = await _cache.GetOrCreateAsync<List<SuggestionDisplayDto>>(CacheKeys.AllSuggestions, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);

                var suggestions = await _repository.GetAllSuggestionsAsync();

                return suggestions.Select(s => new SuggestionDisplayDto
                {
                    Text = s.Text,
                    CreatedAt = s.CreatedAt,
                    Categories = s.SuggestionCategoryAssociations
                        .Select(a => a.SuggestionCategory.CategoryName)
                        .ToList()
                }).ToList();
            });

            var filtered = allSuggestions!
                .Where(s => s.Categories.Any(c => categories.Contains(c)))
                .ToList();

            return filtered;
        }

        public async Task AddSuggestionAsync(string text, List<string> categories)
        {
            await _repository.AddSuggestionAsync(text, categories);
            _cache.Remove(CacheKeys.AllSuggestions);
        }
    }
}
