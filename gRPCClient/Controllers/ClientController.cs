using System.Threading.Tasks;
using Grpc.Net.Client;
using gRPCServer;
using Microsoft.AspNetCore.Mvc;

namespace gRPCClient.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {


        [HttpGet("[action]")]
        public async Task<IActionResult> Get()
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5212");
            var greeterClient = new Greeter.GreeterClient(channel);
          var result=  await greeterClient.SayHelloAsync(new HelloRequest
            {
                Name = "Tahir"
            });
            return Ok(result.Message);
        }
    }
}
