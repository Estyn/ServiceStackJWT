using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Web;
using Punchard.ServiceStackJWTExample.ServiceModel;
using ServiceStack;

namespace Punchard.ServiceStackJWT.ServiceInterface
{
    public class MyServicesAuth : Service
    {
        [Authenticate]
        public object Any(HelloAuth request)

        {
            Debug.Write(HttpContext.Current.User.Identity.Name);
            var x = new HelloResponse { Result = "Hello, {0}!".Fmt(request.Name) };
            var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
            var claims = identity.Claims;

            x.Claims = new List<string>();
            foreach (var claim in claims)
            {
                x.Claims.Add(claim.Type+" : "+claim.Value);
            }
            return x;
        }
    }
}