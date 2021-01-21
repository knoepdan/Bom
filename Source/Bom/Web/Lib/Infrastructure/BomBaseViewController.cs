using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bom.Web.Lib.Mvc;
using Ch.Knomes.Localization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Bom.Web.Lib.Infrastructure
{
    [Controller]
    public class BomBaseViewController : Controller
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            LayoutData = GetLayoutData();
        }


        public string CurrentLanguage
        {
            get
            {
                var passedLang = this.RouteData.Values[Const.RouteArgumentNames.Lang] as string;
                if (passedLang != null && UiGlobals.LocalizationStore != null && UiGlobals.LocalizationStore.HasTranslationsForLangCode(passedLang, false))
                {
                    return passedLang;
                }
                return Const.DefaultLang;
            }
        }

        protected LayoutData LayoutData
        {
            get
            {
                return LayoutDataUtility.LayoutData(this.ViewData);
            }
            set
            {
                LayoutDataUtility.SetLayoutData(this.ViewData, value);
            }
        }

        protected virtual LayoutData GetLayoutData()
        {
            IHtmlService htmlService = new DummyTextservice();
            if (UiGlobals.LocalizationStore != null)
            {
                var resolver = new Ch.Knomes.Localization.Resolver.CustomTextResolver(this.CurrentLanguage);
                htmlService = new Textservice(UiGlobals.LocalizationStore, resolver);
            }
            var layoutData = new LayoutData(htmlService);
            return layoutData;
        }
    }
}