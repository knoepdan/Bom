using Microsoft.AspNetCore.Html;
using System.Web;
using Knomes.Localize.Utils;
using Knomes.Localize.Resolver;

namespace Knomes.Localize
{
    /// <summary>
    /// Dummy implementention of Text-/Htmlservice not translating anything but more or less returning the fallback value
    /// </summary>
    /// <remarks>When HtmlString is returned, values are encoded</remarks>
    public class DummyTextservice : ITextService, IHtmlService
    {
        public DummyTextservice(bool alwaysUseFallback = true)
        {
            this.AlwaysUseFallback = alwaysUseFallback;
            this.Resolver = new CurrentThreadTextResolver(); // doesnt really matter which type of resolver because this is just a summy
        }

        public bool AlwaysUseFallback { get; }

        #region ITextService

        public string Localize(string code, string fallbackValue, params object[] args)
        {
            if (AlwaysUseFallback)
            {
                return LocalizationUtility.FormatStringFailsafe(fallbackValue, args);
            }
            return $"{code}: {LocalizationUtility.FormatStringFailsafe(fallbackValue, args)}";
        }

        public string Fixed(string textValue, params object[] args)
        {
            return LocalizationUtility.FormatStringFailsafe(textValue, args);
        }

        public string Todo(string code, string fallbackValue, params object[] args)
        {
            if (AlwaysUseFallback)
            {
                return LocalizationUtility.FormatStringFailsafe(fallbackValue, args);
            }
            return $"TODO-{code}: {LocalizationUtility.FormatStringFailsafe(fallbackValue, args)}";
        }

        public ITextResolver Resolver { get; }

        public bool HasTranslation(string code)
        {
            return false;
        }

        public bool HasAnyTranslation(string code)
        {
            return false;
        }

        #endregion

        #region IHtmlService

        public HtmlString LocalizeHtml(string code, string htmlFallbackValue, params object[] args)
        {
            var encodedParams = LocalizationUtility.EncodedParams(args);
            var htmlText = LocalizationUtility.FormatStringFailsafe(htmlFallbackValue, encodedParams);

            if (AlwaysUseFallback)
            {
                return new HtmlString(htmlText);
            }
            return new HtmlString($"{HttpUtility.HtmlEncode(code)}: {htmlText}");
        }

        public HtmlString FixedHtml(string htmlText, params object[] args)
        {
            var encodedParams = LocalizationUtility.EncodedParams(args);
            var htmlTextWithParams = LocalizationUtility.FormatStringFailsafe(htmlText, encodedParams);
            return new HtmlString(htmlTextWithParams);
        }

        public HtmlString TodoHtml(string code, string htmlFallbackValue, params object[] args)
        {
            var encodedParams = LocalizationUtility.EncodedParams(args);
            var htmlText = LocalizationUtility.FormatStringFailsafe(htmlFallbackValue, encodedParams);

            if (AlwaysUseFallback)
            {
                return new HtmlString(htmlText);
            }
            return new HtmlString($"Todo-{HttpUtility.HtmlEncode(code)}: {htmlText}");
        }

        #endregion

    }
}