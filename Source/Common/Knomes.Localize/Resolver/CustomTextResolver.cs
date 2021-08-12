using System;
using Knomes.Localize.Utils;

namespace Knomes.Localize.Resolver
{
    public class CustomTextResolver : TextResolverBase
    {
        public CustomTextResolver(string langCode)
        {
            if (string.IsNullOrWhiteSpace(langCode))
            {
                throw new ArgumentException("Passed langCode may not be null or empty", nameof(langCode));
            }
            if (!LocalizationUtility.IsProbablyValidLanguageCode(langCode))
            {
                throw new ArgumentException("Passed langCode is not valid: " + langCode, nameof(langCode));
            }
            this.LanguageCode = LocalizationUtility.TrimmLangCodeForComparisons(langCode);  // "en" or "en-us"
        }

        public string LanguageCode { get; private set; }

        internal void SetLanguageCode(string langCode)
        {
            if (!LocalizationUtility.IsProbablyValidLanguageCode(langCode))
            {
                throw new ArgumentException("Passed langCode is not valid: " + langCode, nameof(langCode));
            }
            this.LanguageCode = langCode;
        }
        public override string GetCurrentLangCode()
        {
            return this.LanguageCode;
        }

        public override string ToString()
        {
            return $"TextResolver: {this.LanguageCode}";
        }

        #region language switch

        public override ITemporaryLanguageSwitch? GetTemporayLanguageSwitch(string langCode)
        {
            if (string.IsNullOrEmpty(langCode))
            {
                throw new ArgumentException("Passed empty langCode", nameof(langCode));
            }
            var langSwitch = new CustomTextResolverSwitch(langCode, this);
            return langSwitch;
        }

        /// <summary>
        /// Temparariyl changes the LanguageCode of passed Resolver is used for localization by the CurrentThreadTextResolver. Attention: always use in a using statement
        /// </summary>
        /// <remarks>
        /// Usage: 
        /// 
        /// using(var switch = resolver.GetTemporaryLanguageSwitch("en")){
        ///     TextService.Localize("someCode", ".....);
        /// }
        /// or
        /// using(var switch = new CustomTextResolver.CustomTextResolverSwitch("en", resolver){
        ///     TextService.Localize("someCode", ".....);
        /// }
        /// </remarks>
        public class CustomTextResolverSwitch : ITemporaryLanguageSwitch
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

        #endregion
    }
}
