using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ch.Knomes.Localization.Store;

namespace Bom.Web.Lib
{
    public static class UiGlobals
    {
        public static ILocalizationStore? LocalizationStore { get; internal set; }
    }
}
