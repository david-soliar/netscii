using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;

namespace netscii.Utils
{
    public static class CaptchaGenerator
    {
        private static readonly int Width = 100;
        private static readonly int Height = 30;
        private static readonly Font Font = SystemFonts.CreateFont("Arial", 16, FontStyle.Bold);
        private const string Characters = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

        public static string GenerateCaptchaBase64Image(string captchaText)
        {
            using var image = new Image<Rgba32>(Width, Height);
            image.Mutate(ctx =>
            {
                ctx.Fill(Color.LightGray);
                ctx.DrawText(captchaText, Font, Color.Black, new PointF(8, 5));

                var rnd = new Random();

                var points = new List<PointF>();
                for (int x = 0; x < Width; x += 5)
                {
                    float y = 15 + (float)(Math.Sin(x * 0.2 + rnd.NextDouble()) * 5);
                    points.Add(new PointF(x, y));
                }
                ctx.DrawLine(Color.DarkGray, 1.5f, points.ToArray());

                for (int i = 0; i < 60; i++)
                {
                    int x = rnd.Next(Width);
                    int y = rnd.Next(Height);
                    ctx.Fill(
                        Color.FromRgb((byte)rnd.Next(255), (byte)rnd.Next(255), (byte)rnd.Next(255)),
                        new Rectangle(x, y, 1, 1)
                    );
                }
            });

            using var ms = new MemoryStream();
            image.SaveAsPng(ms);
            return "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
        }


        public static string GenerateRandomText(int length)
        {
            var rnd = new Random();
            return new string(Enumerable.Range(0, length).Select(_ => Characters[rnd.Next(Characters.Length)]).ToArray());
        }
    }
}
