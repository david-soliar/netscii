using netscii.Models;

namespace netscii.Services.Interfaces
{
    public interface IConversionService
    {
        string FormatName { get; }
        Task<string> ConvertAsync(FormRequest request);
    }

    public interface IHTMLConversionService : IConversionService { }
    public interface IMDConversionService : IConversionService { }
    public interface IANSIConversionService : IConversionService { }
    public interface ILATEXConversionService : IConversionService { }
    public interface IRTFConversionService : IConversionService { }
}
