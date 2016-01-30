using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace Punchcard.ServiceStackJWT
{
    public static class JsonWebToken
    {
        private const string NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
        private const string RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
        private const string ActorClaimType = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor";
        private const string StringClaimValueType = "http://www.w3.org/2001/XMLSchema#string";

        public static ClaimsPrincipal ValidateToken(string token, X509Certificate2 certificate, string audience = null, string issuer = null)
        {

            var claims = ValidateIdentityTokenAsync(token, audience, certificate);

            return new ClaimsPrincipal(ClaimsIdentityFromJwt(claims, issuer));
        }



        private static ClaimsIdentity ClaimsIdentityFromJwt(IEnumerable<Claim> claims, string issuer)
        {
            var subject = new ClaimsIdentity("Federation", NameClaimType, RoleClaimType);
            //var claims = ClaimsFromJwt(jwtData, issuer);

            foreach (Claim claim in claims)
            {
                var type = claim.Type;
                if (type == ActorClaimType)
                {
                    if (subject.Actor != null)
                    {
                        throw new InvalidOperationException(string.Format(
                            "Jwt10401: Only a single 'Actor' is supported. Found second claim of type: '{0}', value: '{1}'", new object[] { "actor", claim.Value }));
                    }

                    subject.AddClaim(new Claim(type, claim.Value, claim.ValueType, issuer, issuer, subject));

                    continue;
                }
                if (type == "name")
                {
                    subject.AddClaim(new Claim(NameClaimType, claim.Value, StringClaimValueType, issuer, issuer));
                    continue;
                }
                if (type == "role")
                {
                    subject.AddClaim(new Claim(RoleClaimType, claim.Value, StringClaimValueType, issuer, issuer));
                    continue;
                }
                var newClaim = new Claim(type, claim.Value, claim.ValueType, issuer, issuer, subject);

                foreach (var prop in claim.Properties)
                {
                    newClaim.Properties.Add(prop);
                }

                subject.AddClaim(newClaim);
            }

            return subject;
        }

        private static IEnumerable<Claim> ValidateIdentityTokenAsync(string token, string audience, X509Certificate2 certificate)
        {
            var parameters = new TokenValidationParameters
            {
                ValidAudience = audience,
                ValidIssuer = "http://localhost:22530",
                IssuerSigningToken = new X509SecurityToken(certificate)

            };

            var handler = new JwtSecurityTokenHandler();
            SecurityToken jwt;
            var id = handler.ValidateToken(token, parameters, out jwt);
            
            return id.Claims;
        }


    }
}