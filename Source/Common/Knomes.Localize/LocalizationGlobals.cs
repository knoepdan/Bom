using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knomes.Localize
{
    public static class LocalizationGlobals
    {
        public static Func<ITextService>? GetDefaultTextServiceFunc { get; set; } = null;


 
    }
}
