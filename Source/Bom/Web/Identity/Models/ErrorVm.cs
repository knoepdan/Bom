using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bom.Web.Identity.Models
{
    public class ErrorVm
    {
        public ErrorVm(Exception? error, bool showDebug)
        {
            if(error == null)
            {
                error = new Exception("No Exception available");
            }
            this.Error = error;
            this.ShowDebugInfo = showDebug;
        }

        public Exception Error { get; }

        public bool ShowDebugInfo { get; set; }

    }
}
