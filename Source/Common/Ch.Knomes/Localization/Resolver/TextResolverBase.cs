using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ch.Knomes.Localization.Store;

namespace Ch.Knomes.Localization.Resolver
{
    public abstract class TextResolverBase : ITextResolver
    {
        public ITextItem? GetTextItem(IEnumerable<ITextItem> translations)
        {
            var currentLang = GetCurrentLangCode();
            var item = GetTextItemByLanguage(currentLang, translations);
            return item;
        }

        private ITextItem? GetTextItemByLanguage(string langCode, IEnumerable<ITextItem> translations)
        {
            var item = translations.FirstOrDefault(x => x.LangCode == langCode);
            if (item == null)
            {
                var parentLangCode = this.GetParentLanguageCode(langCode); // (eg: "en-us"-> "en")
                item = translations.FirstOrDefault(x => x.LangCode == parentLangCode);
            }
            return item;
        }

        public abstract string GetCurrentLangCode();

        /// <summary>
        /// Transforms the language code into a more general form (eg: "de-ch" -> "de")
        /// </summary>
        ///<remarks>Currently only supports step from 2 level to one level (eg: "en-us" -> "en")</remarks>
        protected string? GetParentLanguageCode(string langCode)
        {
            if (langCode != null && langCode.Length > 2)
            {
                // passed langauge had local culture -> fallback to language only  (eg: "en-us" -> "en")
                return langCode.Substring(0, 2);
            }
            return null;
        }

        public abstract ITemporaryLanguageSwitch? GetTemporayLanguageSwitch(string langCode);
    }
}
