using Microsoft.AspNetCore.Mvc;
using netscii.Constants;
using netscii.Models.ViewModels;
using netscii.Services;
using netscii.Services.Factories;

namespace netscii.Controllers.Web
{
    [Route("suggestions")]
    public class SuggestionController : BaseController
    {
        private readonly SuggestionService _suggestionService;
        private readonly SuggestionViewModelFactory _viewModelFactory;

        public SuggestionController(ConversionService conversionService, SuggestionService suggestionService, SuggestionViewModelFactory viewModelFactory) : base(conversionService)
        {
            _suggestionService = suggestionService;
            _viewModelFactory = viewModelFactory;
        }

        private bool IsCaptchaValid(SuggestionViewModel model)
        {
            var expectedCaptcha = HttpContext.Session.GetString("CaptchaCode") ?? "";
            return string.Equals(model.CaptchaCode, expectedCaptcha, StringComparison.OrdinalIgnoreCase);
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery(Name = "SelectedCategories")] List<string> categories)
        {
            return await ExecuteSafe(async () =>
                {
                    bool isInitialRequest = !Request.Query.ContainsKey("SelectedCategories");

                    var allCategories = await _suggestionService.GetCategoryNamesAsync();
                    var selectedCategories = isInitialRequest
                        ? allCategories
                        : (categories.Where(c => !string.IsNullOrWhiteSpace(c)).ToList() ?? new List<string>());

                    var model = await _viewModelFactory.CreateWithSuggestionsAsync(selectedCategories);
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
                    var model = await _viewModelFactory.CreateWithCaptchaAsync(allCategories);

                    HttpContext.Session.SetString("CaptchaCode", model.CaptchaCode);
                    model.CaptchaCode = string.Empty;

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
                    if (!IsCaptchaValid(model))
                    {
                        await _viewModelFactory.RepopulateCaptchaAsync(model);
                       
                        HttpContext.Session.SetString("CaptchaCode", model.CaptchaCode);
                        model.CaptchaCode = string.Empty;

                        model.CaptchaMessage = ExceptionMessages.BadCaptcha;

                        return View(model);
                    }

                    if (model.SelectedCategories == null || !model.SelectedCategories.Any())
                    {
                        await _viewModelFactory.RepopulateCaptchaAsync(model);
                        model.CaptchaMessage = ExceptionMessages.NoCategory;
                        return View(model);
                    }

                    if (string.IsNullOrEmpty(model.Text))
                    {
                        await _viewModelFactory.RepopulateCaptchaAsync(model);
                        model.CaptchaMessage = ExceptionMessages.NoText;
                        return View(model);
                    }

                    await _suggestionService.AddSuggestionAsync(model.Text, model.SelectedCategories);
                    return RedirectToAction("Index");
                },
                renderErrorView: true
            );
        }
    }
}
