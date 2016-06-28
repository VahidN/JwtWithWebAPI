using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;
using JwtWithWebAPI.Services;

namespace JwtWithWebAPI.JsonWebTokenConfig
{
    public class JwtAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Using Func here, creates transient IUsersService's
        /// </summary>
        public Func<IUsersService> UsersService { set; get; }


        /// <summary>
        /// Using Func here, creates transient ITokenStoreService's
        /// </summary>
        public Func<ITokenStoreService> TokenStoreService { set; get; }


        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (skipAuthorization(actionContext))
            {
                return;
            }

            var accessToken = actionContext.Request.Headers.Authorization.Parameter;
            if (string.IsNullOrWhiteSpace(accessToken) ||
                accessToken.Equals("undefined", StringComparison.OrdinalIgnoreCase))
            {
                // null token
                this.HandleUnauthorizedRequest(actionContext);
                return;
            }

            var claimsIdentity = actionContext.RequestContext.Principal.Identity as ClaimsIdentity;
            if (claimsIdentity == null)
            {
                // this is not our issued token
                this.HandleUnauthorizedRequest(actionContext);
                return;
            }

            var userId = claimsIdentity.FindFirst(ClaimTypes.UserData).Value;

            var serialNumberClaim = claimsIdentity.FindFirst(ClaimTypes.SerialNumber);
            if (serialNumberClaim == null)
            {
                // this is not our issued token
                this.HandleUnauthorizedRequest(actionContext);
                return;
            }

            var serialNumber = UsersService().GetSerialNumber(int.Parse(userId));
            if (serialNumber != serialNumberClaim.Value)
            {
                // user has changed its password/roles/stat/IsActive
                this.HandleUnauthorizedRequest(actionContext);
                return;
            }

            if (!TokenStoreService().IsValidToken(accessToken, int.Parse(userId)))
            {
                // this is not our issued token
                this.HandleUnauthorizedRequest(actionContext);
                return;
            }

            base.OnAuthorization(actionContext);
        }

        private static bool skipAuthorization(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                   || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }
    }
}