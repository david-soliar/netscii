using Microsoft.EntityFrameworkCore;
using netscii.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class FontRepository
{
    private readonly NetsciiContext _context;

    public FontRepository(NetsciiContext context)
    {
        _context = context;
    }

    public async Task<List<string>> GetFontsByFormatAsync(string format)
    {
        return await _context.Fonts
                             .Where(f => f.Format == format)
                             .Select(f => f.Name)
                             .ToListAsync();
    }

    public async Task<Dictionary<string, List<string>>> GetFontsAllAsync()
    {
        return await _context.Fonts
                             .GroupBy(f => f.Format)
                             .ToDictionaryAsync(
                                 g => g.Key,
                                 g => g.Select(f => f.Name).ToList()
                             );
    }
}
