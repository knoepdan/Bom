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

//using Microsoft.AspNetCore.JsonPatch;//rmatter.Json;

using System.Text.Encodings.Web;
//using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
//using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Bom.Web.Common.ErrorHandling;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;

using Bom.Core.Common;

using Microsoft.EntityFrameworkCore;

namespace Bom.Web.Identity
{
    public static class IdentityConfig
    {
        public static void ConfigureIdentityServices(IServiceCollection services)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme;//GoogleDefaults.AuthenticationScheme;
                })
               .AddCookie()

               .AddFacebook(options =>
               {
                   FacebookHelper.SetupFacebookAuthentication(options);
               });

            /* // normal authentication via Cookies
            services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth", config =>
                {
                    config.Cookie.Name = "GrandmaasCookie";
                    config.LoginPath = "/Home/Authenticate";
                });
            */

        }

    }
}
