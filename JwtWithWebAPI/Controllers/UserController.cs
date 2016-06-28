using System.Security.Claims;
using System.Web.Http;
using JwtWithWebAPI.JsonWebTokenConfig;
using JwtWithWebAPI.Services;

namespace JwtWithWebAPI.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        private readonly ITokenStoreService _tokenStoreService;

        public UserController(ITokenStoreService tokenStoreService)
        {
            _tokenStoreService = tokenStoreService;
        }

        [JwtAuthorize]
        [Route("logout")]
        [HttpGet, HttpPost]
        public IHttpActionResult Logout()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.UserData).Value;

            // The OWIN OAuth implementation does not support "revoke OAuth token" (logout) by design.
            // Delete the user's tokens from the database (revoke its bearer token)
            _tokenStoreService.InvalidateUserTokens(int.Parse(userId));
            _tokenStoreService.DeleteExpiredTokens();

            return this.Ok(new { message = "Logout successful." });
        }
    }
}