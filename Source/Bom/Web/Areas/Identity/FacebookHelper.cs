using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Bom.Web.Lib.Infrastructure.ErrorHandling;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;

namespace Bom.Web.Areas.Identity
{
    public class FacebookHelper
    {
        public static void SetupFacebookAuthentication(FacebookOptions options)
        {
            // AppId, AppSecret seems to be the same as ClientId and AppSecret
            options.AppId = "180262713784873";//Configuration["Authentication:Facebook:AppId"];
            options.AppSecret = "accfcaff24f4615bc05924b84c9b841c";//Configuration["Authentication:Facebook:AppSecret"];

            options.RemoteAuthenticationTimeout = TimeSpan.FromSeconds(10);
            options.Events.OnCreatingTicket = ctx =>
            {
                Console.WriteLine("FB OnCreating Ticket");
                return Task.CompletedTask;
            };
            options.Events.OnRedirectToAuthorizationEndpoint = ctx =>
            {
                var options = (Microsoft.AspNetCore.Authentication.Facebook.FacebookOptions)ctx.Options;

                // AppId/AppSecret can be set here but it doesnt have an immediate effect. It seems it will only have en effect upon the next login call
                // -> idea.. -> when appID has an invalid value "X" set the correct value from the database, then try a redirect to a page that also requires authentication
                options.AppId = "180262713784873";
                options.AppSecret = "accfcaff24f4615bc05924b84c9b841c";


                Console.WriteLine("FB OnRedirectToAuthorizationEndpoint");

                ctx.Response.Redirect(ctx.RedirectUri); // important
                return Task.CompletedTask;
            };
            options.Events.OnTicketReceived = ctx =>
            {
                Console.WriteLine("FB OnTicketReceived");
                return Task.CompletedTask;
            };
            options.Events.OnRemoteFailure = ctx =>
            {
                Console.WriteLine("FB OnRemoteFailure");
                return Task.CompletedTask;
            };
            options.Events.OnAccessDenied = ctx =>
            {
                Console.WriteLine("FB OnAccessDenied");
                return Task.CompletedTask;
            };

        }


    }
}
