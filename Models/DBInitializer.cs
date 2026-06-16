using netscii.Models.Entities;

namespace netscii.Models
{
    public static class DBInitializer
    {
        public static void Initialize(NetsciiContext context)
        {
            try
            {
                if (!context.Fonts.Any())
                {
                    var fonts = new[]
                    {
                        new Font { Format = "html", Name = "monospace" },
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

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
