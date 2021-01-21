using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ch.Knomes.Localization.Resolver
{

    public interface IGetTemporaryLanguageSwitch
    {
        /// <summary>
        /// returns a language switch to temporarily switch the language of the resolver (returns null if the resolver doesn't support this feature)
        /// </summary>
        /// <remarks>
        /// Usage: 
        /// using(var switch = resolver.GetTemporaryLanguageSwitch("en)){
        ///     TextService.Localize("someCode", ".....);
        /// }
        /// </remarks>
        ITemporaryLanguageSwitch? GetTemporayLanguageSwitch(string langCode);

    }
}
