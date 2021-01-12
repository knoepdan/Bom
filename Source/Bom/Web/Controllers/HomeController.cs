using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bom.Web.Areas.Main.Models;
using Bom.Web.Lib.Infrastructure;
using Bom.Web.Areas.Identity.Models;

namespace Bom.Web.Controllers
{
    [Route("/")]
    [Route("Home")]
    [Controller]
    public class HomeController : BomBaseViewController
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
