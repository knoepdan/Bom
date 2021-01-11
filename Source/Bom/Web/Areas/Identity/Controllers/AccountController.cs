using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bom.Core.Data;
using Bom.Web.Areas.Main.Models;
using Bom.Web.Lib.Infrastructure;
using Bom.Web.Areas.Identity.Models;

namespace Bom.Web.Areas.Main.Controllers
{
    //   [Area("Main")]
    [Area("Identity")]
    [Route("account")]
    [Controller]
    public class AccountController : BomBaseViewController
    {
        private readonly ModelContext _context;

        public AccountController(ModelContext context)
        {
            _context = context;
        }

        // GET: api/Paths
        [HttpGet("register")]
        public IActionResult Register()
        {
            var model = new RegisterVm();

            return View( model);
            //  return View("~/Areas/Identity/Views/Account/Register", model);
        }

    }
}