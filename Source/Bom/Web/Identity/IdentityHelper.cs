using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

using Bom.Core.Identity.DbModels;

namespace Bom.Web.Identity
{
    public class IdentityHelper
    {
        public static OAuthInfo? GetOAuthInfo(IEnumerable<System.Security.Claims.ClaimsIdentity> claimsIdentities, OAuthInfo.OAuthType searchForType)
        {
            switch (searchForType)
            {
                case OAuthInfo.OAuthType.Facebook:
                    return GetFacebookType(claimsIdentities);
                default:
                    return null;
            }
        }



        public static OAuthInfo? GetFacebookType(IEnumerable<System.Security.Claims.ClaimsIdentity> claimsIdentities)
        {

            var fbIdentity = claimsIdentities.FirstOrDefault(x => x.IsAuthenticated && x.AuthenticationType == "Facebook");
            if (fbIdentity != null)
            {
                var identifier = fbIdentity.Claims.FirstOrDefault(x => x.Type.EndsWith("nameidentifier"));
                if (identifier != null && !string.IsNullOrWhiteSpace(identifier.Value))
                {
                    var info = new OAuthInfo(OAuthInfo.OAuthType.Facebook, identifier.Value);

                    // Name
                    info.Name = fbIdentity.Name;

                    // email
                    var emailType = GetClaim(fbIdentity.Claims, "emailaddress", "email");
                    if (emailType != null)
                    {
                        info.Email = emailType.Value;
                    }
                    return info;
                }
            }
            return null;
        }

        public static async Task DoWebLogin(Microsoft.AspNetCore.Http.HttpContext context, User user, string origin = "")
        {
           var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Name  + "facebook"),
                new Claim(ClaimTypes.Email, user.Username),
            };
            var identity = new ClaimsIdentity(claims, origin);
            var userPrincipal = new ClaimsPrincipal(new[] { identity });
            await context.SignInAsync(userPrincipal);
        }


        private static System.Security.Claims.Claim? GetClaim(IEnumerable<System.Security.Claims.Claim> claims, params string[] names)
        {
            if (names == null)
            {
                throw new ArgumentNullException(nameof(names));
            }

            var firstParam = names[0];
            var claim = claims.FirstOrDefault(x => x.Type.Equals(firstParam, StringComparison.InvariantCultureIgnoreCase));
            if (claim != null)
            {
                return claim;
            }


            // trial error appraoch
            foreach (var parName in names)
            {
                claim = claims.FirstOrDefault(x => x.Type.EndsWith(parName));
                if (claim != null)
                {
                    return claim;
                }
            }
            return null;
        }

    }
}
