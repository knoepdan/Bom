using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ch.Knomes.Localization;

namespace Bom.Web.Lib.Mvc
{
    public class LayoutData
    {

        public LayoutData(IHtmlService? htmlService)
        {
            if (htmlService == null)
            {
                throw new ArgumentNullException(nameof(htmlService));
            }
            this.HtmlService = htmlService;

        }


        public IHtmlService HtmlService { get; }

    }
}
