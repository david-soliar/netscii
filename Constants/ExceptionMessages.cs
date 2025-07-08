namespace netscii.Constants
{
    public static class ExceptionMessages
    {
        public const string UnsupportedFormat = "Unsupported format";
        public const string ImageRequired = "Image is required.";
        public const string NoCategory = "At least one category must be selected.";
        public const string NoText = "Suggestion text must not be empty.";
        public const string InternalError = "Internal server error";
        public const string BadCaptcha = "Incorrect captcha";
        public const string Error = "An unexpected error occurred.";

        public static readonly Dictionary<int, string> Messages = new()
        {
            { 400, "Bad request." },
            { 401, "Unauthorized." },
            { 403, "Access denied." },
            { 404, "Page not found." },
            { 405, "Method not allowed." },
            { 408, "Request timeout." },
            { 409, "Conflict occurred." },
            { 410, "Resource no longer available." },
            { 415, "Unsupported media type." },
            { 429, "Too many requests." },
            { 500, "Internal server error." },
            { 501, "Not implemented." },
            { 502, "Bad gateway." },
            { 503, "Service unavailable." },
            { 504, "Gateway timeout." }
        };
    }
}
