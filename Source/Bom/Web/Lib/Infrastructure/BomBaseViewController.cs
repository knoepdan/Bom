﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ch.Knomes.Localization;
using Microsoft.AspNetCore.Mvc.Filters;
using Bom.Web.Lib.Mvc;

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
                if (passedLang != null){
                    if (UiGlobals.LocalizationStore != null && UiGlobals.LocalizationStore.HasTranslationsForLangCode(passedLang, false))
                    {
                        return passedLang;
                    }
                }
                else
                {
                    // try to guess language from broser


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
            IHtmlService htmlService;
            if (UiGlobals.LocalizationStore != null)
            {
                var resolver = new Ch.Knomes.Localization.Resolver.CustomTextResolver(this.CurrentLanguage);
                htmlService = new Textservice(UiGlobals.LocalizationStore, resolver);
            }
            else
            {
                htmlService = new DummyTextservice();
            }
         
            // create layout data
            var layoutData = new LayoutData(htmlService, UiGlobals.AvailableLangCodes);
            return layoutData;
        }
    }
}