using Microsoft.Extensions.Caching.Memory;
using netscii.Constants;
using netscii.Repositories;

namespace netscii.Services
{
    public class FontService
    {
        private readonly IMemoryCache _cache;
        private readonly FontRepository _fontRepository;

        public FontService(IMemoryCache cache, FontRepository fontRepository)
        {
            _cache = cache;
            _fontRepository = fontRepository;
        }

        public async Task<List<string>> GetFontsByFormatAsync(string format)
        {
            string cacheKey = CacheKeys.BaseFont + format;
            if (!_cache.TryGetValue(cacheKey, out var cached) || cached is not List<string> result)
            {
                result = await _fontRepository.GetFontsByFormatAsync(format);
                _cache.Set(cacheKey, result, TimeSpan.FromHours(1));
            }
            return result;
        }

        public async Task<Dictionary<string, List<string>>> GetFontsAllAsync()
        {
            if (!_cache.TryGetValue(CacheKeys.AllFonts, out var cached) || cached is not Dictionary<string, List<string>> result)
            {
                result = await _fontRepository.GetFontsAllAsync();
                _cache.Set(CacheKeys.AllFonts, result, TimeSpan.FromHours(1));
            }
            return result;
        }
    }
}
