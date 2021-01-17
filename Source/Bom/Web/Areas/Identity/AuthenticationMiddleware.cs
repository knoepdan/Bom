using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Bom.Web.Areas.Identity
{
    public class AuthenticationMiddleware
    {
        //private const string AuthorizationHeaderKey = "Authorization";
        private readonly RequestDelegate _next;

        /// <summary>
        /// Http Piplene Middleware for authentication and userdata preparation and caching
        /// </summary>
        /// <param name="next"></param>
        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Add identity info
        /// </summary>
        public virtual Task Invoke(HttpContext httpContext)
        {
           
            // To be used for custom authentication (get auth info request and set it to request so it can be used by controllers)
            // httpContext.Items.Add("info about identitssomething", ob);
            // logger.LogDebug("Request authorized for {UserData}", userData);

            return _next.Invoke(httpContext);
        }
    }
}
