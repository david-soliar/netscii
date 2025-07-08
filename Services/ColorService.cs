using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using netscii.Constants;
using netscii.Repositories;

namespace netscii.Services
{
    public class ColorService
    {
        private readonly IMemoryCache _cache;
        private readonly ColorRepository _colorRepository;

        public ColorService(IMemoryCache cache, ColorRepository colorRepository)
        {
            _cache = cache;
            _colorRepository = colorRepository;
        }

        public async Task<Dictionary<string, string>> GetColorsAsync()
        {
            if (!_cache.TryGetValue(CacheKeys.AllColors, out var cached) || cached is not Dictionary<string, string> result)
            {
                result = await _colorRepository.GetColorsAsync();
                _cache.Set(CacheKeys.AllColors, result, TimeSpan.FromHours(1));
            }
            return result;
        }
    }
}
