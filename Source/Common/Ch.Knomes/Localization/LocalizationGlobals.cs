using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ch.Knomes.Localization
{
    public static class LocalizationGlobals
    {
        public static Func<ITextService>? GetDefaultTextServiceFunc { get; set; } = null;


 
    }
}
