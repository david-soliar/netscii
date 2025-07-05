using System;
using Microsoft.EntityFrameworkCore;

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
                        new Font { Format = "HTML", Name = "monospace" },
                        new Font { Format = "HTML", Name = "'Courier', monospace" },
                        new Font { Format = "HTML", Name = "'Courier New', monospace" },
                        new Font { Format = "HTML", Name = "'Monaco', monospace" },
                        new Font { Format = "HTML", Name = "'Consolas', monospace" },
                        new Font { Format = "HTML", Name = "'Andale Mono', monospace" },
                        new Font { Format = "HTML", Name = "'Lucida Console', monospace" },

                        new Font { Format = "SVG", Name = "monospace" },
                        new Font { Format = "SVG", Name = "'Courier', monospace" },
                        new Font { Format = "SVG", Name = "'Courier New', monospace" },
                        new Font { Format = "SVG", Name = "'Monaco', monospace" },
                        new Font { Format = "SVG", Name = "'Consolas', monospace" },
                        new Font { Format = "SVG", Name = "'Andale Mono', monospace" },
                        new Font { Format = "SVG", Name = "'Lucida Console', monospace" },

                        new Font { Format = "LATEX", Name = "courier" },
                        new Font { Format = "LATEX", Name = "inconsolata" },
                        new Font { Format = "LATEX", Name = "luximono" },
                        new Font { Format = "LATEX", Name = "anonymouspro" },
                        new Font { Format = "LATEX", Name = "beramono" },

                        new Font { Format = "RTF", Name = "Lucida Console" },
                        new Font { Format = "RTF", Name = "Courier New" },
                        new Font { Format = "RTF", Name = "Consolas" },
                        new Font { Format = "RTF", Name = "Fixedsys" },

                    };
                    context.Fonts.AddRange(fonts);
                }

                if (!context.OperatingSystems.Any())
                {
                    var operatingSystems = new[]
                    {
                        new OperatingSystem { Name = "Windows Terminal/Powershell" }, // toto nejako lepsie pomenovat
                        new OperatingSystem { Name = "Mac/Linux Shell" }
                    };
                    context.OperatingSystems.AddRange(operatingSystems);
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
