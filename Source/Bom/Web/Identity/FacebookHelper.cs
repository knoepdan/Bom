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
using Bom.Web.Common.ErrorHandling;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;

namespace Bom.Web.Identity
{
    public static class FacebookHelper
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


                var dto = new FacebookInfoDto();
                dto.Identifier = ctx.TokenResponse.AccessToken; // its not this

                var fbIdentity = ctx.Principal?.Identities.FirstOrDefault(x => x.IsAuthenticated && x.AuthenticationType == "Facebook");
                if (fbIdentity != null)
                {
                    dto.Name = fbIdentity.Name;
                    var emailType = fbIdentity.Claims.FirstOrDefault(x => x.Type.EndsWith("emailaddress"));
                    var identifier = ctx.Principal?.Claims.FirstOrDefault(x => x.Type.EndsWith("nameidentifier"));



                    if(fbIdentity == ctx.Identity)
                    {
                        Console.WriteLine("identity and fbIdentity seems to be the same");
                    }
                }



                ctx.HttpContext.Items["FbTicketTmp"] = dto;

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



                if (ctx.Request.Path.HasValue && ctx.Request.Path.Value != null && ctx.Request.Path.Value.Contains("facebook/register"))
                {
                    // only in certain paths to we redirect (and actually trigger authentication)
                    ctx.Response.Redirect(ctx.RedirectUri); // important

                }
                return Task.CompletedTask;
            };
            options.Events.OnTicketReceived = ctx =>
            {
                Console.WriteLine("FB OnTicketReceived");

                var dto = new FacebookInfoDto();
                dto.Identifier = ctx.Result?.Ticket?.Properties?.ToString(); // seems to be always null

                var fbIdentity  = ctx.Principal?.Identities.FirstOrDefault(x => x.IsAuthenticated && x.AuthenticationType == "Facebook");
                if(fbIdentity != null)
                {
                    dto.Name = fbIdentity.Name;
                    var emailType = fbIdentity.Claims.FirstOrDefault(x => x.Type.EndsWith("emailaddress"));
                    var identifier = ctx.Principal?.Claims.FirstOrDefault(x => x.Type.EndsWith("nameidentifier"));
                }
                
                ctx.HttpContext.Items["FbTicket"] = dto;

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


        public static FacebookInfoDto? GetFacebookPreLoginInfo(this HttpContext context)
        {
            object? val;
            if(context.Items.TryGetValue("FbTicketTmp", out val))
            {
                return val as FacebookInfoDto;
            }
            return null;
        }

        public static FacebookInfoDto? GetFacebookLoginInfo(this HttpContext context)
        {
            object? val;
            if (context.Items.TryGetValue("FbTicket", out val))
            {
                return val as FacebookInfoDto;
            }
            return null;
        }


        public class FacebookInfoDto
        {

            public string? Identifier { get; set; }

            public string? Email { get; set; }

            public string? Name { get; set; }
        }

    }
}
