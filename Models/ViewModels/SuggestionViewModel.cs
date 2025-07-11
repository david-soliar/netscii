﻿using netscii.Models.Dto;
using netscii.Models.Entities;

namespace netscii.Models.ViewModels
{
    public class SuggestionViewModel
    {
        public List<string> Categories { get; set; } = new List<string>();
        public List<string> SelectedCategories { get; set; } = new List<string>();
        public List<SuggestionDisplayDto> Suggestions { get; set; } = new List<SuggestionDisplayDto>();
        public string Text { get; set; } = string.Empty;
        public string CaptchaImageBase64 { get; set; } = string.Empty;
        public string CaptchaCode { get; set; } = string.Empty;
        public string CaptchaMessage { get; set; } = string.Empty;
    }
}
