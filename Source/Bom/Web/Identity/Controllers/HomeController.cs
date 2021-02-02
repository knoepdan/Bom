using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bom.Web.Nodes.Models;
using Bom.Web.Common.Infrastructure;
using Bom.Web.Identity.Models;
using Bom.Web.Common;


using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Http.Extensions;

namespace Bom.Web.Identity.Controllers
{
    [Route("/")]
    [Route("/{" + Const.RouteArgumentNames.Lang + "}/home/")]
    //  [Route("/{" + Const.RouteArgumentNames.Lang + "}/home/{" + Const.RouteArgumentNames.Action + "}")] // Important to have action explicitly defined.
    [Route("/{" + Const.RouteArgumentNames.Lang + "}/{" + Const.RouteArgumentNames.Controller + "}/{" + Const.RouteArgumentNames.Action + "}")]
    [Controller]
    public class HomeController : BomBaseViewController
    {

        public IActionResult Index()
        {
            //UriHelper.GetEncodedUrl(httpContext.Request);

            // urls and examples: attention: only the actually passed values are displayed (if not passed but routing still defines them they are not here)
            var url = this.Request.GetDisplayUrl(); // https://localhost/BomApi/de/Home/Index
            var url2 = this.Request.GetEncodedPathAndQuery(); // /BomApi/de/Home/Index
            var url3 = this.Request.GetEncodedUrl();// https://localhost/BomApi/de/Home/Index

            var path = this.Request.Path.Value; // /de/Home/Index
            var basePath = this.Request.PathBase; // /BomApi
            var urlxxxxxx = url;


            // 
            var cccc = new Microsoft.AspNetCore.Mvc.Routing.UrlRouteContext();
            var urlHelper = new Microsoft.AspNetCore.Mvc.Routing.UrlHelper(this.ControllerContext);



            return View(IdentityViewProvider.DefaultView);
        }
    }
}
