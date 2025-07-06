using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using netscii.Models;


namespace netscii.Services
{
    public class FontService
    {
        private readonly IMemoryCache _cache;
        private const string BaseCacheKey = "Fonts";
        private readonly NetsciiContext _context;

        public FontService(IMemoryCache cache, NetsciiContext context)
        {
            _cache = cache;
            _context = context;
        }

        public async Task<List<string>> GetForFormatAsync(string format)
        {
            string CacheKey = BaseCacheKey + format;
            if (!_cache.TryGetValue(CacheKey, out List<string>? result) || result == null)
            {
                result = await _context.Fonts
                                  .Where(f => f.Format == format) // toto do DB managera
                                  .Select(f => f.Name)
                                  .ToListAsync();

                _cache.Set(CacheKey, result, TimeSpan.FromHours(1));
            }
            return result;
        }
    }
}
