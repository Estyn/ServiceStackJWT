using ServiceStack;

namespace Punchard.ServiceStackJWTExample.ServiceModel
{
    [Route("/helloAuth/{Name}")]
    public class HelloAuth : IReturn<HelloResponse>
    {
        public string Name { get; set; }
    }
    
}