using System.Collections.Generic;
using ServiceStack;

namespace Punchard.ServiceStackJWTExample.ServiceModel
{
    [Route("/hello/{Name}")]
    public class Hello : IReturn<HelloResponse>
    {
        public string Name { get; set; }
    }

    public class HelloResponse
    {
        public string Result { get; set; }
        public List<string> Claims { get; set; }
    }
}