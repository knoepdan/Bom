using System;
using Ch.Knomes.Localization.Utils;

namespace Ch.Knomes.Localization.Resolver
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
            this.LanguageCode = langCode.Trim().ToLowerInvariant(); ;
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
        protected override string GetCurrentLangCode()
        {
            return this.LanguageCode;
        }

        public override string ToString()
        {
            return $"TextResolver: {this.LanguageCode}";
        }
    }
}
