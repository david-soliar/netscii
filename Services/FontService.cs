using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using netscii.Models;


namespace netscii.Services
{
    public class FontService
    {
        private readonly IMemoryCache _cache;
        private const string BaseCacheKey = "Fonts";
        private readonly FontRepository _fontRepository;

        public FontService(IMemoryCache cache, FontRepository fontRepository)
        {
            _cache = cache;
            _fontRepository = fontRepository;
        }

        public async Task<List<string>> GetFontsByFormatAsync(string format)
        {
            string CacheKey = BaseCacheKey + format;
            if (!_cache.TryGetValue(CacheKey, out var cached) || cached is not List<string> result)
            {
                result = await _fontRepository.GetFontsByFormatAsync(format);
                _cache.Set(CacheKey, result, TimeSpan.FromHours(1));
            }
            return result;
        }

        public async Task<Dictionary<string, List<string>>> GetFontsAllAsync()
        {
            string CacheKey = BaseCacheKey + "All";
            if (!_cache.TryGetValue(CacheKey, out var cached) || cached is not Dictionary<string, List<string>> result)
            {
                result = await _fontRepository.GetFontsAllAsync();
                _cache.Set(CacheKey, result, TimeSpan.FromHours(1));
            }
            return result;
        }
    }
}
