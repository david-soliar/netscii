using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using netscii.Models;


namespace netscii.Services
{
    public class ColorService
    {
        private readonly IMemoryCache _cache;
        private const string BaseCacheKey = "Colors";
        private readonly ColorRepository _colorRepository;

        public ColorService(IMemoryCache cache, ColorRepository colorRepository)
        {
            _cache = cache;
            _colorRepository = colorRepository;
        }

        public async Task<Dictionary<string, string>> GetColorsAsync()
        {
            string CacheKey = BaseCacheKey;
            if (!_cache.TryGetValue(CacheKey, out var cached) || cached is not Dictionary<string, string> result)
            {
                result = await _colorRepository.GetColorsAsync();
                _cache.Set(CacheKey, result, TimeSpan.FromHours(1));
            }
            return result;
        }
    }
}
