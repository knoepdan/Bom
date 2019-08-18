using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bom.Utils.Error;

namespace Bom.Web.Lib.Infrastructure.ErrorHandling
{
    public class ErrorInfo
    {
        public int ErrorCode { get; set; } = (int)Utils.Error.ErrorCode.UnexpectedError;
        public string Message { get; set; } = "Error has occured";

        public string UserMessage { get; set; }
    }
}
