using netscii.Models.ViewModels;
using netscii.Models;
using Microsoft.EntityFrameworkCore;


namespace netscii.Services.Factories
{
    public class ConversionViewModelFactory
    {
        private readonly FontService _fontService;
        private readonly ConversionService _conversionService;

        public ConversionViewModelFactory(FontService fontService, ConversionService conversionService)
        {
            _fontService = fontService;
            _conversionService = conversionService;
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

            model.Platform = model.Platforms.FirstOrDefault(string.Empty);
            model.Font = model.Fonts.FirstOrDefault(string.Empty);

            return model;
        }

        public async Task Repopulate(ConversionViewModel model, string format)
        {
            model.Platforms = _conversionService.SupportedPlatforms().ToList();
            model.Fonts = await _fontService.GetFontsByFormatAsync(format);
        }
    }
}
