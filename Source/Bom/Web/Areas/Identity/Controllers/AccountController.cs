﻿using System;
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
using Bom.Core.Model.Identity;

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

        [HttpPost("register")]
        //  public async Task<IActionResult> Register(RegisterVm model)
        public IActionResult Register(RegisterVm model)
        {

            if (this.ModelState.IsValid)
            {
                // TODO -> validate confirm pw, validate if email is unique and return proper error message


                var user = new User();
                user.Username = model.Username;
                user.Salt = "bla";
                user.PasswordHash = "xx";
               // this._context.Users.Add(user);

              //  await this._context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }


            return View(model);
            //  return View("~/Areas/Identity/Views/Account/Register", model);
        }

    }
}