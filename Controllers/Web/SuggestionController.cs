using Microsoft.AspNetCore.Mvc;
using netscii.Exceptions;
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
        public async Task<IActionResult> Index([FromQuery(Name = "SelectedCategories")] List<string> categories)
        {
            return await ExecuteSafe(async () =>
                {
                    var allCategories = await _suggestionService.GetCategoryNamesAsync();

                    bool isInitialRequest = !Request.Query.ContainsKey("SelectedCategories");

                    var selectedCategories = isInitialRequest
                        ? allCategories
                        : (categories.Where(c => !string.IsNullOrWhiteSpace(c)).ToList() ?? new List<string>());

                    var suggestions = await _suggestionService.GetSuggestionsByCategoriesAsync(selectedCategories);

                    var model = new SuggestionViewModel
                    {
                        Categories = allCategories,
                        SelectedCategories = selectedCategories,
                        Suggestions = suggestions
                    };
                    return View(model);
                },
                renderErrorView: true
            );
        }

        [Route("add")]
        [HttpGet]
        public async Task<IActionResult> AddSuggestion()
        {
            return await ExecuteSafe(async () =>
                {
                    var allCategories = await _suggestionService.GetCategoryNamesAsync();
                    var model = new SuggestionViewModel
                    {
                        Categories = allCategories,
                        SelectedCategories = allCategories
                    };
                    return View(model);
                },
                renderErrorView: true
            );
        }

        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> AddSuggestion(SuggestionViewModel model)
        {
            return await ExecuteSafe(async () =>
                {
                    if (model.SelectedCategories == null || !model.SelectedCategories.Any())
                        throw new SuggestionException("At least one category must be selected.");

                    if (string.IsNullOrEmpty(model.Text))
                        throw new SuggestionException("Suggestion text must not be empty.");

                    await _suggestionService.AddSuggestionAsync(model.Text, model.SelectedCategories);
                    return RedirectToAction("Index");
                },
                renderErrorView: true
            );
        }
    }
}
