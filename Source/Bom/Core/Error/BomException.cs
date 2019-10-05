using System;
using System.Collections.Generic;
using System.Text;

namespace Bom.Core.Error
{
    public class BomException : Exception
    {
        public BomException(ErrorCode errorCode, string message = "", Exception? innerException = null, string userMessage = "") : base(message, innerException)
        {
            ErrorCode = errorCode;
            UserMessage = userMessage;
        }

        public ErrorCode ErrorCode { get; }

        public string UserMessage { get; set; }
    }
}
