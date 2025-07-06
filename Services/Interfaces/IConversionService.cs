using netscii.Models.ViewModels;

namespace netscii.Services.Interfaces
{
    public interface IConversionService
    {
        Task<string> ConvertAsync(string format, ConversionViewModel request);
    }
}
