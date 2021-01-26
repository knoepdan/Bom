using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
            // set culture in thread (necessary for attribute localization, and generally good to be consistent) 
            Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo(CurrentLanguage);
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
            Bom.Utils.Dev.Todo("make sure we set the culture before this code is called (and probably CurrentLanguage logic) because attribute logic does not work like this)");


            base.OnActionExecuting(context);
            LayoutData = GetLayoutData();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            if (context.Exception != null)
            {

                Bom.Utils.Dev.Todo("Log error"); // because here 

                // Redirect to error page
                context.ExceptionHandled = true;
                const string errorView = "~/Views/Shared/Error.cshtml";
                context.Result = View(errorView, context.Exception);
            }
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
            IHtmlService? htmlService;
            htmlService = UiGlobals.GetTextservice(CurrentLanguage);
            if (htmlService == null)
            {
                htmlService = new DummyTextservice();
            }
         
            // create layout data
            var layoutData = new LayoutData(htmlService, UiGlobals.AvailableLangCodes);
            return layoutData;
        }
    }
}