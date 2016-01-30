using Funq;
using Punchard.ServiceStackJWT.ServiceInterface;
using Punchcard.ServiceStackJWT;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Razor;

namespace Punchard.ServiceStackJWTExample
{
    public class AppHost : AppHostBase
    {
        /// <summary>
        /// Default constructor.
        /// Base constructor requires a name and assembly to locate web service classes. 
        /// </summary>
        public AppHost(): base("Punchard.ServiceStackJWT", typeof(MyServices).Assembly)
        {

        }

        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources utilized by your web service classes.
        /// </summary>
        /// <param name="container"></param>
        public override void Configure(Container container)
        {
            //Config examples
            this.Plugins.Add(new PostmanFeature());
            this.Plugins.Add(new CorsFeature(allowCredentials:true, allowedHeaders: "Content-Type, Allow, Authorization"));

            this.Plugins.Add(new RazorFormat());

            //For an access token Audience should be http://{issuer}/resouces
            //For an id_token the Audience should the client_id
            Plugins.Add(new AuthFeature(() => new AuthUserSession(),
           new IAuthProvider[] {
                new JsonWebTokenAuthProvider("http://localhost:22530/" + ".well-known/openid-configuration", "http://localhost:22530/resources"),
           }));
            //setup authentication?
        }
    }
}