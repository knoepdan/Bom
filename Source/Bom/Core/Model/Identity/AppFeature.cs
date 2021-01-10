using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bom.Core.Model.Identity
{
    [Flags]
    public enum AppFeature : int
    {
        SeeOther = 1,

        EditOther = 2,
    
        Review = 4,

        Administrator = 8
    }
}
