using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Bom.Core.Identity;
using Bom.Web.Common.Controllers;

namespace Bom.Web.Identity
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        private const string AuthorizationHeaderKey = "Authorization";

        private TokenService _tokenService;

        /// <summary>
        /// Http Piplene Middleware for authentication and userdata preparation and caching
        /// </summary>
        /// <param name="next"></param>
        public AuthenticationMiddleware(RequestDelegate next)
        {
            this._next = next;
            this._tokenService = new TokenService();
        }

        /// <summary>
        /// Add identity info
        /// </summary>
        public virtual Task Invoke(HttpContext httpContext)
        {

            // To be used for custom authentication (get auth info request and set it to request so it can be used by controllers)
            // httpContext.Items.Add("info about identitssomething", ob);
            // logger.LogDebug("Request authorized for {UserData}", userData);


            AuthenticationRequestData? requestData = null;
            var authHeader = httpContext.Request.Headers[AuthorizationHeaderKey];
            var isValidBasicAuthRequest = AuthenticationHeaderValue.TryParse(authHeader, out AuthenticationHeaderValue? authHeaderValue) &&
                                         DecodeBasicAuthRequestData(authHeaderValue, out requestData);
            if (isValidBasicAuthRequest)
            {
                var user = this._tokenService.GetUser(requestData?.Token);
                if (user != null)
                {
                    httpContext.Items.Add(BomBaseController.CurrentUserItemsKey, user);
                }
            }
            return _next.Invoke(httpContext);
        }


        public static bool DecodeBasicAuthRequestData(AuthenticationHeaderValue? authHeaderValue, out AuthenticationRequestData authData)
        {
            if (authHeaderValue == null || string.IsNullOrEmpty(authHeaderValue.Parameter))
            {
                authData = new AuthenticationRequestData(null);
                return false;
            }
            const string bearerKey = "Bearer";
            var isValidBasicAuth = authHeaderValue.Scheme.Equals(bearerKey, StringComparison.OrdinalIgnoreCase) &&
                !string.IsNullOrWhiteSpace(authHeaderValue?.Parameter);

            if (!isValidBasicAuth)
            {
                authData = new AuthenticationRequestData(authHeaderValue?.Parameter ?? "");
                return false;
            }
            else
            {
                authData = new AuthenticationRequestData(authHeaderValue?.Parameter);
            }
            return authData.HasData;
        }
    }

    public class AuthenticationRequestData
    {
        public AuthenticationRequestData(string? token)
        {
            Token = token;
        }

        public string? Token { get; }

        public bool HasData { get { return !string.IsNullOrWhiteSpace(Token); } }

        public override string ToString()
        {
            return $"{nameof(AuthenticationRequestData)} Token: {Token}, HasData: {HasData}";
        }
    }
}
