using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Knomes.Localize.Store;

namespace Knomes.Localize.Resolver
{
    public interface ITextResolver : IGetTemporaryLanguageSwitch
    {
        /// <summary>
        /// Returns text item for the correct language (depending on the revolver with fallbacks or defaults)
        /// </summary>
        ITextItem? GetTextItem(IEnumerable<ITextItem> translations);

        /// <summary>
        /// Returns current language Code (example: "en" or "en-us"
        /// </summary>
        string GetCurrentLangCode();
    }
}
