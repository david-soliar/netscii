using Microsoft.EntityFrameworkCore;
using netscii.Models.Entities;


namespace netscii.Models
{
    public static class DBInitializer
    {
        public static void Initialize(NetsciiContext context)
        {
            try
            {

                context.Database.Migrate();


                if (!context.Colors.Any())
                {
                    var colors = new[]
                    {
                        new Color { Name = "Black",       Hex = "#000000" },
                        new Color { Name = "White",       Hex = "#FFFFFF" },
                        new Color { Name = "Red",         Hex = "#FF0000" },
                        new Color { Name = "Lime",        Hex = "#00FF00" },
                        new Color { Name = "Blue",        Hex = "#0000FF" },
                        new Color { Name = "Yellow",      Hex = "#FFFF00" },
                        new Color { Name = "Cyan",        Hex = "#00FFFF" },
                        new Color { Name = "Magenta",     Hex = "#FF00FF" },
                        new Color { Name = "Silver",      Hex = "#C0C0C0" },
                        new Color { Name = "Gray",        Hex = "#808080" },
                        new Color { Name = "Maroon",      Hex = "#800000" },
                        new Color { Name = "Olive",       Hex = "#808000" },
                        new Color { Name = "Green",       Hex = "#008000" },
                        new Color { Name = "Purple",      Hex = "#800080" },
                        new Color { Name = "Teal",        Hex = "#008080" },
                        new Color { Name = "Navy",        Hex = "#000080" },
                        new Color { Name = "Orange",      Hex = "#FFA500" },
                        new Color { Name = "Pink",        Hex = "#FFC0CB" },
                        new Color { Name = "Brown",       Hex = "#A52A2A" },
                        new Color { Name = "Gold",        Hex = "#FFD700" }
                    };
                    context.Colors.AddRange(colors);
                }

                if (!context.Fonts.Any())
                {
                    var fonts = new[]
                    {
                        new Font { Format = "html", Name = "monospace" },       //pridat alias pre page rendering
                        new Font { Format = "html", Name = "'Courier', monospace" },
                        new Font { Format = "html", Name = "'Courier New', monospace" },
                        new Font { Format = "html", Name = "'Monaco', monospace" },
                        new Font { Format = "html", Name = "'Consolas', monospace" },
                        new Font { Format = "html", Name = "'Andale Mono', monospace" },
                        new Font { Format = "html", Name = "'Lucida Console', monospace" },

                        new Font { Format = "svg", Name = "monospace" },
                        new Font { Format = "svg", Name = "'Courier', monospace" },
                        new Font { Format = "svg", Name = "'Courier New', monospace" },
                        new Font { Format = "svg", Name = "'Monaco', monospace" },
                        new Font { Format = "svg", Name = "'Consolas', monospace" },
                        new Font { Format = "svg", Name = "'Andale Mono', monospace" },
                        new Font { Format = "svg", Name = "'Lucida Console', monospace" },

                        new Font { Format = "latex", Name = "courier" },
                        new Font { Format = "latex", Name = "inconsolata" },
                        new Font { Format = "latex", Name = "luximono" },
                        new Font { Format = "latex", Name = "anonymouspro" },
                        new Font { Format = "latex", Name = "beramono" },

                        new Font { Format = "rtf", Name = "Lucida Console" },
                        new Font { Format = "rtf", Name = "Courier New" },
                        new Font { Format = "rtf", Name = "Consolas" },
                        new Font { Format = "rtf", Name = "Fixedsys" },

                    };
                    context.Fonts.AddRange(fonts);
                }

                if (!context.SuggestionCategories.Any())
                {
                    var categories = new[]
                    {
                        new SuggestionCategory { CategoryName = "General" },
                        new SuggestionCategory { CategoryName = "Bug" },
                        new SuggestionCategory { CategoryName = "Feature Request" },
                        new SuggestionCategory { CategoryName = "UI" },
                        new SuggestionCategory { CategoryName = "Performance" }
                    };
                    context.SuggestionCategories.AddRange(categories);
                }


                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); //toto by mal byt log
                throw;
            }
        }
    }
}
