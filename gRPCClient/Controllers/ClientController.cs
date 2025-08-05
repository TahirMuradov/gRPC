using System.Text.Json;
using System.Threading;
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
            var result = await greeterClient.SayHelloAsync(new HelloRequest
            {
                Name = "Tahir"
            });
            return Ok(result.Message);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetMessage()
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5212");
            var streamMessage = new StreamMessage.StreamMessageClient(channel);
            var responseStream = streamMessage.SayHello(new MessageRequest
            {
                Name = "Tahir"
            });
            CancellationToken cancellationToken = new CancellationToken();
            List<MessageReply> streamDatas = new  List<MessageReply>();
            while (await responseStream.ResponseStream.MoveNext(cancellationToken))
            {
                Console.WriteLine(responseStream.ResponseStream.Current.Message + " " + responseStream.ResponseStream.Current.Index);
                streamDatas.Add(responseStream.ResponseStream.Current);
            }
            return Ok(JsonSerializer.Serialize(streamDatas));
        }
    }
}
