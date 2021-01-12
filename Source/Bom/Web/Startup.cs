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
using Bom.Web.Lib.Infrastructure.ErrorHandling;


using Bom.Core.Common;

using Microsoft.EntityFrameworkCore;

namespace Bom.Web
{
    public class Startup
    {
        private readonly ILogger _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option => option.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
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
                    var requestInfo = ErrorUtility.GetRequestInfo(context, false);
                    if (exFeature != null && exFeature.Error != null)
                    {
                        _logger.LogError(exFeature.Error, $"Error (Path: {exFeature.Path}, requestInfo: {requestInfo})");
                    }
                    else
                    {
                        _logger.LogError($"Unknown error: {requestInfo}");
                    }
                        
                    // request
                    await ErrorUtility.SetErrorResponse(context);
                });
            });

            app.UseHttpsRedirection();
            //app.UseStaticFiles(); // only needed to server help
            //app.UseCookiePolicy();


            app.UseRouting();

            //app.UseAuthorization(); // must be between UseRouting and UseEndpoints

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
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
