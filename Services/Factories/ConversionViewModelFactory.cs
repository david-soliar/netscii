using netscii.Constants;
using netscii.Models.ViewModels;


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

        public async Task<ConversionViewModel> CreateWithDefaultsAsync(string format)
        {
            string characters = ConversionConstants.Characters.GetValueOrDefault(format, string.Empty);

            var model = new ConversionViewModel
            {
                Format = format,
                Characters = characters
            };

            await RepopulateAsync(model, format);

            return model;
        }

        public async Task RepopulateAsync(ConversionViewModel model, string format)
        {
            model.Platforms = _conversionService.SupportedPlatforms().ToList();
            model.Fonts = await _fontService.GetFontsByFormatAsync(format);
        }
    }
}
