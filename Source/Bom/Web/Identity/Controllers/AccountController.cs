using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ch.Knomes.Security;
using Bom.Core.Common;
using Bom.Web.Nodes.Models;
using Bom.Web.Common.Infrastructure;
using Bom.Web.Identity.Models;
using Bom.Core.Identity;
using Bom.Core.Identity.DbModels;
using Bom.Web.Common;

namespace Bom.Web.Identity.Controllers
{
    [Area("Identity")]
    [Route("/")]
    [Route("/{" + Const.RouteArgumentNames.Lang + "}/{" + Const.RouteArgumentNames.Controller + "}")]
    [Controller]
    public class AccountController : BomBaseViewController
    {

        private readonly ModelContext _context;

        public AccountController(ModelContext context)
        {
            _context = context;
        }


        [HttpGet("")]
        public IActionResult Index()
        {
            return Login();
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            if (this.User != null)
            {
                this.SignOut();
            }
            return View(IdentityViewProvider.AccountLogin);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginVm loginModel)
        {
            if (this.ModelState.IsValid)
            {
                //this.SignIn();   or HttpContext.SignInAsync
                // https://www.youtube.com/watch?v=Fhfvbl_KbWo
            }
            return View(IdentityViewProvider.AccountLogin);
        }

        [HttpGet("usernametaken")]
        public IActionResult UsernameTaken()
        {
            return Login();
        }


        [HttpGet("logout")]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            if (this.User != null)
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

            return View(IdentityViewProvider.AccountRegister, model);
            //  return View("~/Areas/Identity/Views/Account/Register", model);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterVm model)
        {
            // Validation
            model.Username = model.Username == null ? "" : model.Username.Trim();

            if (this.ModelState.IsValid)
            {
                if (model.Password != model.ConfirmPassword)
                {
                    this.ModelState.AddModelError(nameof(model.Password), this.TextService.Localize("Account.Register.PasswordNotConfirmed", "Entered passwords do not match."));
                }
                bool alreadyExists = this.ModelState.IsValid && await this._context.Users.AnyAsync(x => x.Username == model.Username); // only do db request if anything else is ok (minor performance improvment)
                if (alreadyExists)
                {
                    this.ModelState.AddModelError(nameof(model.Username), this.TextService.Localize("Account.Register.UsernameNotUnique", "Username is already registered."));
                }
            }

            if (this.ModelState.IsValid)
            {
                // create user
                var pwHelper = new PasswordHelper();
                var pwResult = pwHelper.HashPasswordWithRandomSalt(model.Password, true);

                var user = new User();
                user.Username = model.Username.Trim();
                user.Salt = pwResult.SaltString;
                user.PasswordHash = pwResult.HashString;
                user.ActivationToken = Guid.NewGuid() + DateTime.Now.Ticks.ToString() + CryptoUtility.GetPseudoRandomString(5);
                this._context.Users.Add(user);
                await this._context.SaveChangesAsync();

                // TODO send email

                return RedirectToAction("Index", "Home");
            }
            return View(IdentityViewProvider.AccountRegister, model);
            //  return View("~/Areas/Identity/Views/Account/Register", model);
        }

    }
}