using Microsoft.EntityFrameworkCore;
using netscii.Models;
using netscii.Models.Entities;

namespace netscii.Repositories
{
    public class SuggestionRepository
    {
        private readonly NetsciiContext _context;

        public SuggestionRepository(NetsciiContext context)
        {
            _context = context;
        }

        public async Task AddSuggestionAsync(string text, List<string> categories)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var suggestion = new Suggestion
                {
                    Text = text
                };

                _context.Suggestions.Add(suggestion);
                await _context.SaveChangesAsync();

                var categoryEntities = await _context.SuggestionCategories
                    .Where(c => categories.Contains(c.CategoryName))
                    .ToListAsync();

                foreach (var category in categoryEntities)
                {
                    _context.SuggestionCategoryAssociations.Add(new SuggestionCategoryAssociation
                    {
                        SuggestionId = suggestion.Id,
                        SuggestionCategoryId = category.Id
                    });
                }

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<string>> GetCategoryNamesAsync()
        {
            return await _context.SuggestionCategories
                .Select(c => c.CategoryName)
                .ToListAsync();
        }

        public async Task<List<Suggestion>> GetAllSuggestionsAsync()
        {
            return await _context.Suggestions
                .Include(s => s.SuggestionCategoryAssociations)
                    .ThenInclude(sa => sa.SuggestionCategory)
                .ToListAsync();
        }

        public async Task<List<Suggestion>> GetSuggestionsByCategoryAsync(List<string> categories)
        {
            return await _context.Suggestions
                .Where(s => s.SuggestionCategoryAssociations
                    .Any(sa => categories.Contains(sa.SuggestionCategory.CategoryName)))
                .Include(s => s.SuggestionCategoryAssociations)
                    .ThenInclude(sa => sa.SuggestionCategory)
                .ToListAsync();
        }
    }
}
