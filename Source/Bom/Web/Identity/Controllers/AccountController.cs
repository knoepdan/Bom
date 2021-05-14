using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ch.Knomes.Security;
using Bom.Core.Common;
using Bom.Core.Identity.DbModels;
using Bom.Web.Nodes.Models;
using Bom.Web.Common.Infrastructure;
using Bom.Web.Identity.Models;
using Bom.Core.Identity;
using Bom.Web.Common;
using Microsoft.AspNetCore.Authentication;

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
        public async Task<IActionResult> Login(LoginVm loginModel)
        {
            if (this.ModelState.IsValid)
            {
                var userName = loginModel.Username?.Trim();
                var user = await this._context.Users.FirstOrDefaultAsync(x => x.Username == userName);
                if (user == null)
                {
                    this.ModelState.AddModelError(nameof(loginModel.Username), this.TextService.Localize("Account.Login.UsernameNotFound", "User not found"));
                    return View(IdentityViewProvider.AccountLogin);
                }
                else
                {

                    Utils.Dev.Todo("Remove this dummy check.. just for testing during developmetn");
                    if (userName != "daniel.knoepfel@live.de")
                    {

                        var pwHelper = new PasswordHelper();
                        var pwResult = pwHelper.HashPassword(loginModel.Password, user.Salt ?? "");
                        if (user.PasswordHash != pwResult.HashString)
                        {
                            this.ModelState.AddModelError(nameof(loginModel.Username), this.TextService.Localize("Account.Login.PwInvalid", "User not found"));
                            return View(IdentityViewProvider.AccountLogin);
                        }
                    }

                }
                await DoLogin(user);
                var linkProvider = new IdentityLinkProvider(this);
                return Redirect(linkProvider.HomeLink);// RedirectToAction("Index", "Home");

            }
            return View(IdentityViewProvider.AccountLogin);
        }

        [HttpGet("usernametaken")]
        public IActionResult UsernameTaken()
        {
            return Login();
        }


   //     [HttpGet("logout")]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            if (this.User != null)
            {
                await this.HttpContext.SignOutAsync();
                //this.SignOut();
            }
            var linkProvider = new IdentityLinkProvider(this);
            return Redirect(linkProvider.HomeLink);// RedirectToAction("Index", "Home");
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
                var pwResult = pwHelper.HashPasswordWithRandomSalt(model.Password);

                var user = new User();
                user.Username = model.Username.Trim();
                user.Salt = pwResult.SaltString;
                user.PasswordHash = pwResult.HashString;
                user.ActivationToken = Guid.NewGuid() + DateTime.Now.Ticks.ToString() + CryptoUtility.GetPseudoRandomString(5);
                this._context.Users.Add(user);
                await this._context.SaveChangesAsync();

                // TODO send email etc.
                var linkProvider = new IdentityLinkProvider(this);
                return Redirect(linkProvider.HomeLink);// RedirectToAction("Index", "Home");
            }
            return View(IdentityViewProvider.AccountRegister, model);

        }

        private async Task DoLogin(User user)
        {
            Utils.Dev.Todo("improve.. ");

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Name  + "aaaaaaa"),
                new Claim(ClaimTypes.Email, user.Username),
            };
            var identity = new ClaimsIdentity(claims, "id");
            var userPrincipal = new ClaimsPrincipal(new[] { identity });
            await this.HttpContext.SignInAsync(userPrincipal);
        }

    }
}