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

namespace Bom.Web
{
    public class Startup
    {
        private readonly ILogger _logger;

        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, ILogger<Startup> logger, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            _logger = logger;
            _env = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
          //  services.AddMvc(option => option.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));


            var connection = Configuration.GetConnectionString("DbContext"); // @"Server=(localdb)\mssqllocaldb;Database=EFGetStarted.AspNetCore.NewDb;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<ModelContext>
                (options => 
                {
                    options.UseSqlServer(connection);
                    options.UseLazyLoadingProxies();
                });

            // authentication

            //----------
            Identity.IdentityConfig.ConfigureIdentityServices(services);

            var ccsFiles = new string[] { "/css/normalize.css", "/css/macros.css", "/css/varsAndDefault.css", "/css/layout.css", "/css/mainClasses.css" }; // also defines order
            var webRoot = this._env.WebRootPath; // example:  "C:\\Dev\\Github\\Bom\\Source\\Bom\\Web\\wwwroot";
            var provider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(webRoot); // since .net core 6, we have to explicitly specify webRoot (probably a but that will be fixed some day)

#if DEBUG
            // controller with views needed third party authentication/redirects
            Utils.Dev.Todo("Razor precompilation not working anymore in .Net 6.0. Reactivate it once it works again (possible a bug of the framework or library)");
            //  TODO: outcommented because this no longer works in .Net Core 6.0 (worked in 5.0)          services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddControllersWithViews();


            services.AddWebOptimizer(pipeline =>
            {
                pipeline.AddCssBundle("/css/bundle.css", ccsFiles).UseFileProvider(provider);
            });

#else
            services.AddControllersWithViews();
            services.AddWebOptimizer(pipeline =>
            {
                pipeline.AddCssBundle("/css/bundle.css", cssFiles).UseFileProvider(provider).MinifyCss();
            }); 
#endif

            //  services.AddWebOptimizer();
            Utils.Dev.PossibleImprovment("Call from a better place", Utils.Dev.ImproveArea.ToCheck, Utils.Dev.Urgency.Low);
            Common.UiGlobals.InitGlobals();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            bool isDeveloptment = env.IsDevelopment();
            if (isDeveloptment)
            {
                _logger.LogInformation("In Development environment");
                app.UseDeveloperExceptionPage();

                app.UseCors("MyPolicy");
            }
            else
            {
                //              app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseExceptionHandler(errorApp =>
            {

                errorApp.Run(async context =>
                {
                    // logging
                    var exFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var requestInfo = ErrorUtility.GetRequestInfo(context, isDeveloptment);
                    if (exFeature != null && exFeature.Error != null)
                    {
                        _logger.LogError(exFeature.Error, $"Error (Path: {exFeature.Path}, requestInfo: {requestInfo})");
                    }
                    else
                    {
                        _logger.LogError($"Unknown error: {requestInfo}");
                    }
                   
                        
                    // request
                    await ErrorUtility.SetErrorResponse(context, isDeveloptment);
                });
            });

            app.UseHttpsRedirection();

            app.UseWebOptimizer(); // must be before use static files (https://github.com/ligershark/WebOptimizer)
            app.UseStaticFiles(); // to load react app
            //app.UseCookiePolicy();


            app.UseRouting();
            app.UseMiddleware<Common.LocalizationMiddleware>(); // set current language


            app.UseAuthentication();   // must be between UseRouting and UseEndpoints
            app.UseMiddleware<Bom.Web.Identity.AuthenticationMiddleware>(); // custom stuff

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                // fallback https://weblog.west-wind.com/posts/2020/Jul/12/Handling-SPA-Fallback-Paths-in-a-Generic-ASPNET-Core-Server
               endpoints.MapFallbackToController("Index", "Home");
              //  endpoints.MapFallback()
           
            });


            /*
            app.UseMvc(routes =>
            {
                routes.MapRoute("areaRoute", "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            */
        }
    }
}
