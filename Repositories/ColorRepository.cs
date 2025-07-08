using Microsoft.EntityFrameworkCore;
using netscii.Models;

namespace netscii.Repositories
{
    public class ColorRepository
    {
        private readonly NetsciiContext _context;

        public ColorRepository(NetsciiContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<string, string>> GetColorsAsync()
        {
            return await _context.Colors
                                 .ToDictionaryAsync(c => c.Name, c => c.Hex);
        }
    }

}
