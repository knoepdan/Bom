using Microsoft.AspNetCore.Html;
using System.Web;
using Ch.Knomes.Localization.Utils;
using Ch.Knomes.Localization.Resolver;

namespace Ch.Knomes.Localization
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

        HtmlString IHtmlService.FixedHtml(string htmlText, params object[] args)
        {
            var encodedParams = LocalizationUtility.EncodedParams(args);
            var htmlTextWithParams = LocalizationUtility.FormatStringFailsafe(htmlText, encodedParams);
            return new HtmlString(htmlTextWithParams);
        }

        HtmlString IHtmlService.TodoHtml(string code, string htmlFallbackValue, params object[] args)
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

        #region IGetTemporaryLanguageSwitch

        public ITemporaryLanguageSwitch? GetTemporayLanguageSwitch(string langCode)
        {
            return new DummySwitch();
        }

        public class DummySwitch: ITemporaryLanguageSwitch
        {
            public void Dispose()
            {
                // nothing to do
            }
        }

        #endregion
    }
}