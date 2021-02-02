using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ch.Knomes.Localization;

namespace Bom.Web.Common.Mvc
{
    public class LayoutData
    {

        public LayoutData(IHtmlService? htmlService, IReadOnlyCollection<string> availableLangCodes)
        {
            if (htmlService == null)
            {
                throw new ArgumentNullException(nameof(htmlService));
            }
            this.HtmlService = htmlService;
            this.AvailableLanguageCodes = availableLangCodes;
        }

        public IHtmlService HtmlService { get; }

        public IReadOnlyCollection<string> AvailableLanguageCodes { get; }

        public string GetCurrentLangCode()
        {
            var code = HtmlService.Resolver.GetCurrentLangCode(); // method is simply convenience
            return code;
        }

    }
}
