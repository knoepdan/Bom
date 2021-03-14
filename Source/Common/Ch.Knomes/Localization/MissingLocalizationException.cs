using System;

namespace Ch.Knomes.Localization
{
    public class MissingLocalizationException : Exception
    {
        public MissingLocalizationException(string message, string code, string langCode) : base(message)
        {
            this.Code = code;
            this.LanguageCode = langCode;
        }

        public string Code { get; }

        public string LanguageCode { get; }
    }
}
