using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ch.Knomes.Security;
using Bom.Core.Common;
using Bom.Web.Areas.Main.Models;
using Bom.Web.Lib.Infrastructure;
using Bom.Web.Areas.Identity.Models;
using Bom.Core.Identity;
using Bom.Core.Identity.DbModels;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Bom.Web.Areas.Main.Controllers
{
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


        [HttpGet("logout")]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            if(this.User != null)
            {
                this.SignOut();
            }

            return RedirectToAction("register");
            //  return View("~/Areas/Identity/Views/Account/Register", model);
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
        public async Task<IActionResult> Register(RegisterVm model)
        {

            if (this.ModelState.IsValid)
            {
                // TODO -> validate confirm pw, validate if email is unique and return proper error message

                var pwHelper = new PasswordHelper();
                var pwResult = pwHelper.HashPasswordWithRandomSalt(model.Password, true);

                var user = new User();
                user.Username = model.Username;
                user.Salt = pwResult.SaltString;
                user.PasswordHash = pwResult.HashString;
                user.ActivationToken = Guid.NewGuid() + DateTime.Now.Ticks.ToString() + CryptoUtility.GetPseudoRandomString(5);
                this._context.Users.Add(user);
                await this._context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            return View(model);
            //  return View("~/Areas/Identity/Views/Account/Register", model);
        }

    }
}