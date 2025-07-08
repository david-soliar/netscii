using Microsoft.AspNetCore.Mvc;
using netscii.Models.Entities;
using netscii.Models.ViewModels;
using netscii.Services;

namespace netscii.Controllers.Web
{
    [Route("suggestions")]
    public class SuggestionController : BaseController
    {
        private readonly SuggestionService _suggestionService;

        public SuggestionController(ConversionService conversionService, SuggestionService suggestionService) : base(conversionService)
        {
            _suggestionService = suggestionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<string> categories = await _suggestionService.GetCategoryNamesAsync();
            var suggestions = await _suggestionService.GetAllSuggestionsAsync();

            var model = new SuggestionViewModel
            {
                Categories = categories,
                SelectedCategories = categories,
                Suggestions = suggestions
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromBody] SuggestionViewModel model)
        {
            List<string> categories = await _suggestionService.GetCategoryNamesAsync();
            var suggestions = await _suggestionService.GetSuggestionsByCategoriesAsync(model.Categories);

            model.Categories = categories;
            model.Suggestions = suggestions;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddSuggestion([FromBody] SuggestionViewModel model)
        {
            List<string> categories = await _suggestionService.GetCategoryNamesAsync();
            var suggestions = await _suggestionService.GetSuggestionsByCategoriesAsync(model.Categories);

            model.Categories = categories;
            model.Suggestions = suggestions;

            await _suggestionService.AddSuggestionAsync(model.Text, model.SelectedCategories);

            return View(model);
        }
    }
}
