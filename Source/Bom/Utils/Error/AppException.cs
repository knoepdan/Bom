using System;
using System.Collections.Generic;
using System.Text;

namespace Bom.Utils.Error
{
    public class AppException : Exception
    {
        public AppException(ErrorCode errorCode, string message = "", Exception innerException = null, string userMessage = "") : base(message, innerException)
        {
            ErrorCode = errorCode;
            UserMessage = userMessage;
        }

        public ErrorCode ErrorCode { get; }

        public string UserMessage { get; set; }
    }
}