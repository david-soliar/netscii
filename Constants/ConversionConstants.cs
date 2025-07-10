namespace netscii.Constants
{
    public static class ConversionConstants
    {
        public static readonly IReadOnlyDictionary<string, string> Characters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["html"] = "#$023456789?Labdeghnopqu_ ",
            ["svg"] = "#$023456789?Labdeghnopqu_ ",
            ["txt"] = "#$023456789?Labdeghnopqu_ ",
            ["latex"] = "MNHUCIi;,.",
            ["rtf"] = "%#+=-:."
        };

        public static readonly IReadOnlyDictionary<string, TimeSpan> Periods = new Dictionary<string, TimeSpan>(StringComparer.OrdinalIgnoreCase)
        {
            ["24h"] = TimeSpan.FromHours(24),
            ["7d"] = TimeSpan.FromDays(7),
            ["30d"] = TimeSpan.FromDays(30),
            ["1y"] = TimeSpan.FromDays(365),
            ["all"] = TimeSpan.MaxValue
        };

        public static readonly IReadOnlyList<string> Formats = Utils.ImageConverters.ConverterDispatcher.Converters.Keys.ToList();
    }
}
