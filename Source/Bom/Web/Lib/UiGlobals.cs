using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ch.Knomes.Localization.Store;

namespace Bom.Web.Lib
{
    public static class UiGlobals
    {
        private static bool isInitialized = false;
        public static void InitGlobals()
        {

            Utils.Dev.Todo("Globals need to initialized (once)");

            if (isInitialized)
            {
                throw new Exception("UiGlobals may only be initialized once");
            }
            isInitialized = true;
        }


        public static IInfoLocalizationStore? LocalizationStore { get; }
    }
}
