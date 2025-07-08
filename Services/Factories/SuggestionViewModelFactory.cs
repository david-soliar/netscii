using netscii.Models.ViewModels;

namespace netscii.Services.Factories
{
    public class SuggestionViewModelFactory
    {
        private readonly SuggestionService _suggestionService;
        private readonly CaptchaService _captchaService;

        public SuggestionViewModelFactory(SuggestionService suggestionService, CaptchaService captchaService)
        {
            _suggestionService = suggestionService;
            _captchaService = captchaService;
        }

        public async Task<SuggestionViewModel> CreateWithSuggestionsAsync(List<string> selectedCategories)
        {
            var allCategories = await _suggestionService.GetCategoryNamesAsync();
            var suggestions = await _suggestionService.GetSuggestionsByCategoriesAsync(selectedCategories);

            var model = new SuggestionViewModel
            {
                Categories = allCategories,
                SelectedCategories = selectedCategories,
                Suggestions = suggestions
            };
            return model;
        }

        public async Task<SuggestionViewModel> CreateWithCaptchaAsync(List<string> selectedCategories)
        {
            var allCategories = await _suggestionService.GetCategoryNamesAsync();

            var model = new SuggestionViewModel
            {
                Categories = allCategories,
                SelectedCategories = selectedCategories
            };
            model.CaptchaCode = _captchaService.GenerateRandomText(6);
            model.CaptchaImageBase64 = await _captchaService.GenerateCaptchaImageAsync(model.CaptchaCode);
            return model;
        }

        public async Task RepopulateCaptchaAsync(SuggestionViewModel model)
        {
            model.Categories = await _suggestionService.GetCategoryNamesAsync();
            model.CaptchaCode = _captchaService.GenerateRandomText(6);
            model.CaptchaImageBase64 = await _captchaService.GenerateCaptchaImageAsync(model.CaptchaCode);
        }
    }
}
