using netscii.Models.ViewModels;

namespace netscii.Services.Interfaces
{
    public interface IConversionViewModelFactory
    {
        Task<ConversionViewModel> CreateWithDefaults(string format);
    }
}
