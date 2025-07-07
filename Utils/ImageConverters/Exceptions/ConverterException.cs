namespace netscii.Utils.ImageConverters.Exceptions
{
    public class ConverterException : Exception
    {
        public ConverterErrorCode ErrorCode { get; }

        public ConverterException(ConverterErrorCode errorCode)
            : base(GetDefaultMessage(errorCode))
        {
            ErrorCode = errorCode;
        }

        public ConverterException(ConverterErrorCode errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public ConverterException(ConverterErrorCode errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        private static string GetDefaultMessage(ConverterErrorCode code)
        {
            return code switch
            {
                ConverterErrorCode.ConversionFailed => "The conversion failed due to an unexpected error.",

                ConverterErrorCode.ImageLoadFailed => "Failed to load the image.",
                ConverterErrorCode.UnsupportedFormat => "The selected format is not supported.",

                ConverterErrorCode.InvalidScale => "Scale must be greater than zero and smaller than width and height of the image.",
                ConverterErrorCode.EmptyCharacterSet => "Character set must not be empty.",

                ConverterErrorCode.InvalidBackgroundColor => "The background color is invalid.",
                ConverterErrorCode.InvalidFont => "The selected font is invalid or unsupported.",

                ConverterErrorCode.UnsupportedPlatform => "The selected operating system is invalid or unsupported.",

                _ => "An unknown error occurred during conversion."
            };
        }
    }

    public enum ConverterErrorCode
    {
        Unknown = 0,
        
        ConversionFailed,

        ImageLoadFailed,
        UnsupportedFormat,

        InvalidScale,
        EmptyCharacterSet,

        InvalidFont,
        InvalidBackgroundColor,

        UnsupportedPlatform
    }
}
