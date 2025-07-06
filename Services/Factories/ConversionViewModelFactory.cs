using netscii.Models.ViewModels;
using netscii.Models;
using Microsoft.EntityFrameworkCore;
using netscii.Services.Interfaces;

namespace netscii.Services.Factories
{
    public class ConversionViewModelFactory : IConversionViewModelFactory
    {
        private readonly NetsciiContext _context;

        public ConversionViewModelFactory(NetsciiContext context)
        {
            _context = context;
        }

        public async Task<ConversionViewModel> CreateWithDefaults(string format)
        {
            string characters = format.ToLowerInvariant() switch
            {
                "html" => "%#*+=-:.",
                "svg" => "%#*+=-:.",
                "txt" => "#$023456789?Labdeghnopqu_",
                "latex" => "MNHUCIi;,.",
                "rtf" => "%#+=-:.",
                _ => string.Empty
            };

            var fontsFromDb = await _context.Fonts
                                  .Where(f => f.Format == format) // toto do DB managera
                                  .Select(f => f.Name)
                                  .ToListAsync();

            var operatingSystemsFromDb = await _context.OperatingSystems
                      .Select(f => f.Name)
                      .ToListAsync();

            var model = new ConversionViewModel
            {
                Controller = format,
                Fonts = fontsFromDb,
                OperatingSystems = operatingSystemsFromDb,
                Characters = characters
            };
            model.Font = fontsFromDb.FirstOrDefault(string.Empty);
            model.OperatingSystem = operatingSystemsFromDb.FirstOrDefault(string.Empty);

            return model;
        }
    }
}
