using System;
using System.Collections.Generic;
using System.Text;

namespace Bom.Core.Error
{
#pragma warning disable CA1032 // Implement standard exception constructors
    public class BomException : Exception
#pragma warning restore CA1032 // Implement standard exception constructors
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
