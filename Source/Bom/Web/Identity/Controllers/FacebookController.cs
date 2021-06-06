using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Authorization;
using Bom.Web.Common;
using Microsoft.AspNetCore.Authentication;

namespace Bom.Web.Identity.Controllers
{
    [Area("Identity")]
    //  [Route("facebook")]
    [Route("/{" + Const.RouteArgumentNames.Lang + "}/{" + Const.RouteArgumentNames.Controller + "}")]
    [Controller]
    public class FacebookController : OAuthBaseController
    {
        public override string ProviderName => "Facebook";

        public FacebookController(ModelContext context) : base(context)
        {
            //        _context = context;
        }


        [Authorize]
        [HttpPost("login")]
        public async Task<IActionResult> Login(object dummy)
        {
            if (!this.ModelState.IsValid)
            {
                throw new Exception("invalid model state");
            }
            var actionResult = await PerformLogin();
            return actionResult;
        }



        [Authorize]
        [HttpGet("login")]
        public async Task<IActionResult> Login()
        {
            if (!this.ModelState.IsValid)
            {
                throw new Exception("invalid model state");
            }
            var actionResult = await PerformLogin();
            return actionResult;
        }



        [Authorize]
        [HttpPost("register")]
        public async Task<IActionResult> Register(object dummy)
        {
            if (!this.ModelState.IsValid)
            {
                throw new Exception("invalid model state");
            }
            var actionResult = await GoToSecondRegisterPage();
            return actionResult;
        }

        [Authorize]
        [HttpGet("register")]
        public async Task<IActionResult> Register()
        {
            // TODO try to find a way prevent dummy request
            if (!this.ModelState.IsValid)
            {
                throw new Exception("invalid model state");
            }

            // will return to this endpoint
            var actionResult = await GoToSecondRegisterPage();
            return actionResult;
        }

        //private async Task<IActionResult> LoginFacebookUser()
        //{
        //    var linkProvider = new IdentityLinkProvider(this);
        //    var oAuthInfo = Bom.Web.Identity.IdentityHelper.GetFacebookType(this.User.Identities);
        //    if (oAuthInfo != null && !string.IsNullOrEmpty(oAuthInfo.Identifier))
        //    {
        //        var username = await this.Context.Users.Where(x => x.FacebookId == oAuthInfo.Identifier).Select(x => x.Username).FirstOrDefaultAsync();
        //        if (!string.IsNullOrEmpty(username))
        //        {
        //            //
        //            //this.SignIn();   or HttpContext.SignInAsync
        //            // https://www.youtube.com/watch?v=Fhfvbl_KbWo

        //            throw new NotImplementedException("LoginFacebookUser");
        //        }
        //        else
        //        {
        //            var notRegiMsg = new Mvc.UserMessage(this.TextService.Localize("Identity.Register.OAuthLoginNotRegisterd", "Login failed because you are not registered yet."), Mvc.UserMessage.MessageType.Info);
        //            this.TempDataHelper.AddMessasge(notRegiMsg);
        //            return Redirect(linkProvider.AccountLoginLink);
        //        }
        //    }

        //    // failed
        //    var msg = new Mvc.UserMessage(this.TextService.Localize("Identity.Register.OAuthLoginFailed", "Login failed. Not found."), Mvc.UserMessage.MessageType.Info);
        //    this.TempDataHelper.AddMessasge(msg);
        //    return Redirect(linkProvider.AccountLoginLink);
        //}

        private async Task<IActionResult> GoToSecondRegisterPage()
        {
            var oAuthInfo = Bom.Web.Identity.IdentityHelper.GetFacebookType(this.User.Identities);
            if (oAuthInfo != null)
            {
                var linkProvider = new IdentityLinkProvider(this);

                string? existingId = null;
                var username = oAuthInfo.Email;
                if (string.IsNullOrEmpty(username))
                {
                    existingId = await this.Context.Users.Where(x => x.FacebookId == oAuthInfo.Identifier).Select(x => x.FacebookId).FirstOrDefaultAsync();
                }
                else
                {
                    var existingUser = await this.Context.Users.Where(x => x.FacebookId == oAuthInfo.Identifier || x.Username == username).FirstOrDefaultAsync();
                    if (existingUser != null)
                    {
                        existingId = existingUser.FacebookId;
                        if (existingUser.Username == oAuthInfo.Email && string.IsNullOrEmpty(existingId))
                        {
                            // email is registered but not via facebook
                            var msg = new Mvc.UserMessage(this.TextService.Localize("Identity.Register.UsernameTaken", "Registration not possible because the provided email address is already used as username."), Mvc.UserMessage.MessageType.Info);
                            this.TempDataHelper.AddMessasge(msg);
                            return Redirect(linkProvider.AccountRegisterLink);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(existingId))
                {
                    return Redirect(linkProvider.OAuthRegisterAlreadyRegisteredLink(this.ProviderName));
                }

                var user = this.CreateNewOAuthUser(oAuthInfo);
                user.FacebookId = oAuthInfo.Identifier;
                this.Context.Users.Add(user);
                await this.Context.SaveChangesAsync();

                // redirect to
                var regVm = new OAuthRegVm(user, "Facebook");
                return Redirect(linkProvider.OAuthRegisterSuccessLink(this.ProviderName));//RedirectToAction("success");
            }
            else
            {
                throw new Exception("Identities not found");  // or just a redirect to normal register??
            }
        }


        private async Task<IActionResult> PerformLogin()
        {
            var linkProvider = new IdentityLinkProvider(this);

            var oAuthInfo = Bom.Web.Identity.IdentityHelper.GetFacebookType(this.User.Identities);
            if (oAuthInfo != null && !string.IsNullOrEmpty(oAuthInfo.Identifier))
            {
                var user = await this.Context.Users.Where(x => x.FacebookId == oAuthInfo.Identifier).FirstOrDefaultAsync();
                if (user == null)
                {
                    var message = new Mvc.UserMessage(this.TextService.Localize("Account.Login.UsernameNotFound", "User not found"), Mvc.UserMessage.MessageType.Info);
                    this.TempDataHelper.AddMessasge(message);
                    return Redirect(linkProvider.AccountLoginLink);
                }
                else
                {
                    await DoLogin(user);
                    return Redirect(linkProvider.AccountLoginLink);
                }
            }
            var msg = new Mvc.UserMessage(this.TextService.Localize("Account.OAuth.NotFoundOrError", "Failed to login"), Mvc.UserMessage.MessageType.Info);
            this.TempDataHelper.AddMessasge(msg);
            return Redirect(linkProvider.AccountLoginLink);
        }


        private async Task DoLogin(User user)
        {
            Utils.Dev.Todo("improve.. ");
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Name  + "facebook"),
                new Claim(ClaimTypes.Email, user.Username),
            };
            var identity = new ClaimsIdentity(claims, "id");
            var userPrincipal = new ClaimsPrincipal(new[] { identity });
            await this.HttpContext.SignInAsync(userPrincipal);
        }

    }
}