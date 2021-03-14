using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ch.Knomes.Localization.Resolver;

namespace Ch.Knomes.Localization
{
    public enum NoLocalizationPolicy : byte
    {
        /// <summary>
        ///  Use the hardcoded fallback
        /// </summary>
        Fallback = 0,

        /// <summary>
        /// The supposedly localized message tells the user that localization has gone wrong
        /// </summary>
        FlagMessasge = 1,

        /// summary>
        /// Throws an exception
        /// </summary>
        ThrowException = 2
    }

    /// <summary>
    /// Textservice that has a specific behavior when no is found
    /// </summary>
    /// <remarks>Usually only for testing or finding untranslated texts, especially for annotations</remarks>
    public class MissingLocalizationFinderTextService : ITextService
    {
        public MissingLocalizationFinderTextService(ITextService textService, NoLocalizationPolicy noLocalizationPolicy)
        {
            if (textService == null)
            {
                throw new ArgumentException(nameof(textService));
            }
            this.InnerTextService = textService;
        }

        public ITextService InnerTextService { get; }

        public NoLocalizationPolicy NoLocalizationPolicy { get; }

        public ITextResolver Resolver => InnerTextService.Resolver;

        public string Fixed(string textValue, params object[] args)
        {
            return this.InnerTextService.Fixed(textValue, args); // // no reason to have a logic here
        }

        public bool HasTranslation(string code)
        {
            return this.InnerTextService.HasTranslation(code);
        }

        public string Localize(string code, string fallbackValue, params object[] args)
        {
            bool hasTrans = this.HasTranslation(code);
            if (hasTrans)
            {
                return this.InnerTextService.Localize(code, fallbackValue, args);
            }

            switch (this.NoLocalizationPolicy)
            {
                case NoLocalizationPolicy.Fallback:
                    return this.InnerTextService.Localize(code, fallbackValue, args);
                case NoLocalizationPolicy.FlagMessasge:
                    return "LOCALIZATINON MISSING: " + this.InnerTextService.Localize(code, fallbackValue, args);
                case NoLocalizationPolicy.ThrowException:
                    var lang = this.Resolver.GetCurrentLangCode();
                    throw new MissingLocalizationException($"Missing Localization for code: '{code}', LangCode:  '{lang}', Fallback: '{fallbackValue}'", code, lang);

                default:
                    throw new Exception($"Unknown NoLocalizationPolicy: " + this.NoLocalizationPolicy);
            }
        }

        public string Todo(string code, string fallbackValue, params object[] args)
        {
            return this.InnerTextService.Todo(code, fallbackValue, args); // no reason to have a logic here
        }
    }
}
