using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Bom.Web.Common
{
    public class LocalizationMiddleware
    {

        private readonly RequestDelegate _next;

        /// <summary>
        /// Http Piplene Middleware for authentication and userdata preparation and caching
        /// </summary>
        /// <param name="next"></param>
        public LocalizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Add identity info
        /// </summary>
        public virtual Task Invoke(HttpContext httpContext)
        {
            var currentLang = GetCurrentLanguage(httpContext.Request);
            Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo(currentLang);
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
            return _next.Invoke(httpContext);
        }


        public string GetCurrentLanguage(HttpRequest request)
        {
            var passedLang = request.RouteValues[Const.RouteArgumentNames.Lang] as string;
            if (passedLang != null)
            {
                if (UiGlobals.LocalizationStore != null && UiGlobals.LocalizationStore.HasTranslationsForLangCode(passedLang, false))
                {
                    return passedLang;
                }
            }
            else
            {
                Bom.Utils.Dev.Todo("guess language to use from browser langauge");
            }
            return Const.DefaultLang;
        }   
        
    }
}
