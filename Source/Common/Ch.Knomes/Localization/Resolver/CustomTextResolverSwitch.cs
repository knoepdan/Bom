using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ch.Knomes.Localization.Utils;

namespace Ch.Knomes.Localization.Resolver
{
    /// <summary>
    /// Temparariyl changes the LanguageCode of passed Resolver is used for localization by the CurrentThreadTextResolver. Attention: always use in a using statement
    /// </summary>
    /// <remarks>
    /// Usage: 
    /// using(var switch = new CustomTextResolverSwitch("en"){
    ///     TextService.Localize("someCode", ".....);
    /// }
    /// </remarks>
    public class CustomTextResolverSwitch : IDisposable
    {
        public CustomTextResolverSwitch(string langCode, CustomTextResolver resolver)
        {
            if (!LocalizationUtility.IsProbablyValidLanguageCode(langCode))
            {
                throw new ArgumentException("Passed langCode is not valid", nameof(langCode));
            }

            if (resolver == null)
            {
                throw new ArgumentNullException(nameof(resolver));
            }

            this._resolver = resolver;
            this._oldLangcode = resolver.LanguageCode;
            this._resolver.SetLanguageCode(langCode); // set new
        }

        private CustomTextResolver _resolver;

        private string _oldLangcode;

        public void Dispose()
        {
            this._resolver.SetLanguageCode(this._oldLangcode);
        }
    }
}
