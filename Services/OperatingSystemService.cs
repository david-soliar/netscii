using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using netscii.Models;


namespace netscii.Services
{
    public class OperatingSystemService
    {
        private readonly IMemoryCache _cache;
        private const string BaseCacheKey = "OperatingSystems";
        private readonly NetsciiContext _context;

        public OperatingSystemService(IMemoryCache cache, NetsciiContext context)
        {
            _cache = cache;
            _context = context;
        }

        public async Task<List<string>> GetForFormatAsync(string format)
        {
            string CacheKey = BaseCacheKey + format;
            if (!_cache.TryGetValue(CacheKey, out List<string>? result) || result == null)
            {
                result = await _context.OperatingSystems
                      .Select(f => f.Name) // toto do DB managera
                      .ToListAsync();

                _cache.Set(CacheKey, result, TimeSpan.FromHours(1));
            }
            return result;
        }
    }
}
