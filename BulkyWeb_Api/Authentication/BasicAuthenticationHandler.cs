﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace BulkyWeb_Api.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return Task.FromResult(AuthenticateResult.Fail("missing Authorization Header"));

            var authHeader = Request.Headers["Authorization"].ToString();
            if (!authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
                return Task.FromResult(AuthenticateResult.Fail("unknown scheme"));
            var encodedCredentials = authHeader["Basic".Length..];
            var decodedCredenials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
            var UsernameAndPassword = decodedCredenials.Split(':');
            if (UsernameAndPassword[0] != "admin" || UsernameAndPassword[1] != "password")
                return Task.FromResult(AuthenticateResult.Fail("Invali Username or Password"));

            var Identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name,UsernameAndPassword[0]),
                new Claim(ClaimTypes.NameIdentifier,"1")

            },"Basic");
            
            var principal = new ClaimsPrincipal(Identity);
            var ticket = new AuthenticationTicket(principal, "Basic");
          
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
