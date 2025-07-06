using Microsoft.AspNetCore.Mvc;
using netscii.Services.Interfaces;

namespace netscii.Controllers.Web
{
    [Route("ansi")]
    public class ANSIController : BaseController
    {
        public ANSIController(IANSIConversionService conversionService, IConversionViewModelFactory viewModelFactory)
            : base(conversionService, viewModelFactory) { }
    }


    [Route("emoji")]
    public class EMOJIController : BaseController
    {
        public EMOJIController(IEMOJIConversionService conversionService, IConversionViewModelFactory viewModelFactory)
            : base(conversionService, viewModelFactory) { }
    }


    [Route("html")]
    public class HTMLController : BaseController
    {
        public HTMLController(IHTMLConversionService conversionService, IConversionViewModelFactory viewModelFactory)
            : base(conversionService, viewModelFactory) { }
    }


    [Route("latex")]
    public class LATEXController : BaseController
    {
        public LATEXController(ILATEXConversionService conversionService, IConversionViewModelFactory viewModelFactory)
            : base(conversionService, viewModelFactory) { }
    }


    [Route("rtf")]
    public class RTFController : BaseController
    {
        public RTFController(IRTFConversionService conversionService, IConversionViewModelFactory viewModelFactory)
            : base(conversionService, viewModelFactory) { }
    }


    [Route("svg")]
    public class SVGController : BaseController
    {
        public SVGController(ISVGConversionService conversionService, IConversionViewModelFactory viewModelFactory)
            : base(conversionService, viewModelFactory) { }
    }


    [Route("txt")]
    public class TXTController : BaseController
    {
        public TXTController(ITXTConversionService conversionService, IConversionViewModelFactory viewModelFactory)
            : base(conversionService, viewModelFactory) { }
    }
}
