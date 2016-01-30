using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Web;
using Microsoft.IdentityModel.Protocols;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Web;

namespace Punchcard.ServiceStackJWT
{
    public class JsonWebTokenAuthProvider : AuthProvider, IAuthWithRequest
    {
        private static string Name = "JWT";
        private static string Realm = "/auth/JWT";
        private const string MissingAuthHeader = "Missing Authorization Header";
        private const string InvalidAuthHeader = "Invalid Authorization Header";


        private string Audience { get; }
        private string Issuer { get; }
        private X509Certificate2 Certificate { get; }

        /// <summary>
        /// Creates a new JsonWebToken Auth Provider
        /// </summary>
        /// <param name="discoveryEndpoint">aThe url to get the configuration informaion from.. (er "http://localhost:22530/" + ".well-known/openid-configuration")</param>
        /// <param name="audience">The client for openID (eg js_oidc)</param>

        public JsonWebTokenAuthProvider(string discoveryEndpoint, string audience = null)
        {
            Provider = Name;
            AuthRealm = Realm;
            Audience = audience;
            
            var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(discoveryEndpoint);

            var config =  configurationManager.GetConfigurationAsync().Result;

            Certificate = new X509Certificate2(Convert.FromBase64String(config.JsonWebKeySet.Keys.First().X5c.First()));
            Issuer = config.Issuer;
        }

        public override object Authenticate(IServiceBase authService, IAuthSession session, Authenticate request)
        {
            var header = request.oauth_token;
           
            // if no auth header, 401
            if (string.IsNullOrEmpty(header))
            {
                throw HttpError.Unauthorized(MissingAuthHeader);
            }

            var headerData = header.Split(' ');

            // if header is missing bearer portion, 401
            if (string.Compare(headerData[0], "BEARER", StringComparison.OrdinalIgnoreCase) != 0)
            {
                throw HttpError.Unauthorized(InvalidAuthHeader);
            }

            try
            {
               
                // set current principal to the validated token principal
                Thread.CurrentPrincipal = JsonWebToken.ValidateToken(headerData[1], Certificate,  Audience,  Issuer);

                if (HttpContext.Current != null)
                {
                    // set the current request's user the the decoded principal
                    HttpContext.Current.User = Thread.CurrentPrincipal;
                }

                // set the session's username to the logged in user
                session.UserName = Thread.CurrentPrincipal.Identity.Name;

                return OnAuthenticated(authService, session, new AuthTokens(), new Dictionary<string, string>());
            }
            catch (Exception ex)
            {
                throw new HttpError(HttpStatusCode.Unauthorized, ex);
            }
        }

        
        /// <param name="session"></param>
        /// <param name="tokens"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public override bool IsAuthorized(IAuthSession session, IAuthTokens tokens, Authenticate request = null)
        {
            return HttpContext.Current.User.Identity.IsAuthenticated && session.IsAuthenticated && string.Equals(session.UserName, HttpContext.Current.User.Identity.Name, StringComparison.OrdinalIgnoreCase);
        }

        public void PreAuthenticate(IRequest request, IResponse response)
        {
            var header = request.Headers["Authorization"];
            var authService = request.TryResolve<AuthenticateService>();
            authService.Request = request;

            // pass auth header in as oauth token to authentication
            authService.Post(new Authenticate
            {
                provider = Name,
                oauth_token = header
            });
        }
        
    }
}