using Microsoft.AspNetCore.Mvc;
using netscii.Models.ViewModels;

namespace netscii.ViewComponents
{
    public class ConversionFormViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ConversionViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "ConversionViewModel passed to ViewComponent is null");
            }

            var items = ViewContext.HttpContext.Items;

            items["netscii.Title"] = $"Convert image to {model.Controller}";
            items["netscii.InjectedScripts"] = "<script src='/js/conversion-form.js'></script>";
            items["netscii.InjectedStyles"] = "<link rel='stylesheet' href='/css/conversion-form.css' />";

            return View(model);
        }
    }
}
