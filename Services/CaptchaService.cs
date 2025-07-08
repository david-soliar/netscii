using netscii.Utils;

namespace netscii.Services
{
    public class CaptchaService
    {
        public async Task<string> GenerateCaptchaImageAsync(string captchaText)
        {
            return await Task.Run(() => CaptchaGenerator.GenerateCaptchaBase64Image(captchaText));
        }

        public string GenerateRandomText(int length)
        {
            return CaptchaGenerator.GenerateRandomText(length);
        }
    }
}
