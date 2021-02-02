using System;
using System.Collections.Generic;
using System.Text;

namespace Bom.Core.Common.Error
{
    public enum ErrorCode
    {
        NoError = 0,
        Forbidden = 3000,
        NotFound = 4000,
        UnexpectedError = 5000,
    }
}