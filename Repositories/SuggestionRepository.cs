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

        public async Task AddSuggestionAsync(Suggestion suggestion, List<int> categoryIds)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.Suggestions.Add(suggestion);
                await _context.SaveChangesAsync();

                foreach (var categoryId in categoryIds)
                {
                    _context.SuggestionCategoryAssociations.Add(new SuggestionCategoryAssociation
                    {
                        SuggestionId = suggestion.Id,
                        SuggestionCategoryId = categoryId
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
