using netscii.Models;
using netscii.Services.Interfaces;

namespace netscii.Services
{
    public class EMOJIConversionService : IEMOJIConversionService
    {
        public string FormatName => "emoji";

        public Task<string> ConvertAsync(ConversionViewModel request)
        {
            return Task.Run(() =>
            {
                using var stream = request.GetImageStream();
                return Utils.Converter.EMOJI(
                    stream,
                    request.Characters,
                    request.Scale,
                    request.Invert);
            });
        }
    }
}
