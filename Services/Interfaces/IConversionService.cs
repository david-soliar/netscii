using netscii.Models;

namespace netscii.Services.Interfaces
{
    public interface IConversionService
    {
        string FormatName { get; }
        Task<string> ConvertAsync(FormRequest request);
    }
}
