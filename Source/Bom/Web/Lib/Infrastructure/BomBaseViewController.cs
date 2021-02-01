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
            base.OnActionExecuting(context);
            LayoutData = GetLayoutData();
            this.TempDataHelper = new TempDataHelper(this.TempData);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            if (context.Exception != null)
            {

                Bom.Utils.Dev.Todo("Log error"); // because it wont get to the normal error handling

                // Redirect to error page
                context.ExceptionHandled = true;
                const string errorView = "~/Views/Shared/Error.cshtml";
                context.Result = View(errorView, context.Exception);
            }
        }

        protected TempDataHelper TempDataHelper { get; private set; } = default!;


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
            htmlService = UiGlobals.GetTextservice();
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