using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ch.Knomes.Localization;
using Microsoft.AspNetCore.Mvc.Filters;
using Bom.Web.Identity.Mvc;
using Bom.Web.Common;

namespace Bom.Web.Identity.Controllers
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
                context.ExceptionHandled = true;


                // Redirect to error page  (possible improvment: redirect instead of just showing error page)
                bool isDebug = false;
#if DEBUG
                Bom.Utils.Dev.Todo("Set variable depending on config and not on preprocessor", Bom.Utils.Dev.Urgency.Low);
                isDebug = true;
#endif
                const string errorView = Identity.IdentityViewProvider.SharedErorr; // "~/Views/Shared/Error.cshtml";
                var errorModel = new Identity.Models.ErrorVm(context.Exception, isDebug);
                context.Result = View(errorView, errorModel);
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

        private LayoutData GetLayoutData()
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

        protected ITextService TextService => UiGlobals.GetTextservice() as ITextService ?? new DummyTextservice();
    }
}