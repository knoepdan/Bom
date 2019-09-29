using System;
using System.Collections.Generic;
using System.Text;

namespace Bom.Core.TestUtils
{
    public static class DbLockers
    {
        public static readonly object DbLock = new object();
    }
}
