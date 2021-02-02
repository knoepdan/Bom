using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bom.Core.Common.Error;

namespace Bom.Web.Common.ErrorHandling
{
    public class ErrorInfo
    {
        public int ErrorCode { get; set; } = (int)Core.Common.Error.ErrorCode.UnexpectedError;
        public string Message { get; set; } = "Error has occured";

        public string UserMessage { get; set; } = "";
    }
}