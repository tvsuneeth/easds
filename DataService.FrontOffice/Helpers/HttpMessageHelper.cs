using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace twg.chk.DataService.FrontOffice.Helpers
{
    public enum ErrorMessageCode { FatalError = 5, Info = 1 };

    public static class HttpMessageHelper
    {

        public static ErrorMessage GenerateErrorMessage(String errorMessage)
        {
            return GenerateErrorMessage(errorMessage, ErrorMessageCode.Info);
        }

        public static ErrorMessage GenerateErrorMessage(String errorMessage, ErrorMessageCode errorCode)
        {
            var errorMessageObj = new ErrorMessage
            {
                Message = errorMessage,
                ErrorCode = (int)errorCode,
                UserFriendlyErrorCodeText = Enum.GetName(typeof(ErrorMessageCode),errorCode.ToString())
            };

            return errorMessageObj;
        }
    }

    public class ErrorMessage
    {
        public String Message { get; set; }
        public String UserFriendlyErrorCodeText { get; set; }
        public int ErrorCode { get; set; }
    }
}