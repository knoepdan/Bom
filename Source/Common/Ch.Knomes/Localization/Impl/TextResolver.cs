using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ch.Knomes.Localization.Impl
{
    public interface ITextResolver
    {
        ITextItem? GetTextItem(IEnumerable<ITextItem> translations);

        ITextItem? GetTextItemByLanguage(string langCode, IEnumerable<ITextItem> translations);
    }

    internal class TextResolver : ITextResolver
    {
        public ITextItem? GetTextItemByLanguage(string langCode, IEnumerable<ITextItem> translations)
        {
            var item = translations.FirstOrDefault(x => x.LangCode == langCode);
            if (item == null && langCode.Length > 2)
            {
                // passed langauge had local culture -> fallback to language only  (eg: "en-us" -> "en")
                item = translations.FirstOrDefault(x => x.LangCode == langCode.Substring(0, 2));
            }
            return item;
        }

        public ITextItem? GetTextItem(IEnumerable<ITextItem> translations)
        {
            var currentLang = GetCurrentLangCode();
            var item = GetTextItemByLanguage(currentLang, translations);
            return item;
        }


        private string GetCurrentLangCode()
        {
            var transCulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            if (transCulture != null)
            {
                return transCulture.Name.ToLowerInvariant(); // "en" or "en-us" ;
            }
            return "en";
        }
    }
}
