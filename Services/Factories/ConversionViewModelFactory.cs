using netscii.Models.ViewModels;
using netscii.Models;
using Microsoft.EntityFrameworkCore;


namespace netscii.Services.Factories
{
    public class ConversionViewModelFactory
    {
        private readonly FontService _fontService;
        private readonly OperatingSystemService _operatingSystemService;

        public ConversionViewModelFactory(FontService fontService, OperatingSystemService operatingSystemService)
        {
            _fontService = fontService;
            _operatingSystemService = operatingSystemService;
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

            var model = new ConversionViewModel
            {
                Format = format,
                Characters = characters
            };

            await Repopulate(model, format);

            model.Font = model.Fonts.FirstOrDefault(string.Empty);
            model.OperatingSystem = model.OperatingSystems.FirstOrDefault(string.Empty);

            return model;
        }

        public async Task Repopulate(ConversionViewModel model, string format)
        {
            model.Fonts = await _fontService.GetForFormatAsync(format);
            model.OperatingSystems = await _operatingSystemService.GetForFormatAsync(format);
        }
    }
}
