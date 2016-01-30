using Punchard.ServiceStackJWTExample.ServiceModel;
using ServiceStack;

namespace Punchard.ServiceStackJWT.ServiceInterface
{
    public class MyServices : Service
    {
        public object Any(Hello request)
        {
            return new HelloResponse { Result = "Hello, {0}!".Fmt(request.Name) };
        }
    }
}