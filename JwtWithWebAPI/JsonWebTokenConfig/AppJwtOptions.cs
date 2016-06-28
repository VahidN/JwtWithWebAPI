using System;
using System.Text;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;

namespace JwtWithWebAPI.JsonWebTokenConfig
{
    public class AppJwtOptions : JwtBearerAuthenticationOptions
    {
        public AppJwtOptions(IAppJwtConfiguration config)
        {
            this.AuthenticationMode = AuthenticationMode.Active;
            this.AllowedAudiences = new[] { config.JwtAudience };
            this.IssuerSecurityTokenProviders = new[]
            {
                new SymmetricKeyIssuerSecurityTokenProvider(
                    issuer: config.JwtIssuer,
                    base64Key: Convert.ToBase64String(Encoding.UTF8.GetBytes(config.JwtKey)))
            };
        }
    }
}