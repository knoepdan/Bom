using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ch.Knomes.Localization.Store;
using Ch.Knomes.Localization.Utils;

namespace Bom.Web.Lib
{
    public static class UiGlobals
    {
        private static bool isInitialized = false;
        public static void InitGlobals()
        {
            if (isInitialized)
            {
                throw new Exception("UiGlobals may only be initialized once");
            }
            isInitialized = true;

            LocalizationStore = GetLocalizationStore();
            if (LocalizationStore != null)
            {
                var allCodes = LocalizationStore.GetAvailableLanguageCodes();
                var langCodes = allCodes.Where(x => !string.IsNullOrEmpty(x)).Select(x => LocalizationUtility.TrimToLanguagePart(x));
                if (langCodes.Any())
                {
                    AvailableLangCodes = langCodes.Distinct().ToArray();
                }
            }
        }

        public static IInfoLocalizationStore? LocalizationStore { get; private set; }

        public static IReadOnlyCollection<string> AvailableLangCodes { get; private set; } = new List<string>() { Const.DefaultLang };

        public static IInfoLocalizationStore GetLocalizationStore()
        {
            Bom.Utils.Dev.Todo("really init the translation store (not some dummy initialization)");
            var translations = new List<ITextItemWithCode>();
            translations.Add(new CodeTextItem("Common.Save", "en", "Save")); // Code, LangCode, Text,
            translations.Add(new CodeTextItem("Common.Save", "de", "Speicher"));
            translations.Add(new CodeTextItem("Common.Cancel", "en", "Cancel"));
            translations.Add(new CodeTextItem("Common.Cancel", "de", "Abbrechen"));
            var store = new LocalizationStore(translations);
            return store;
        }
    }
}
