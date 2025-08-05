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
            List<MessageReply> streamDatas = new List<MessageReply>();
            while (await responseStream.ResponseStream.MoveNext(cancellationToken))
            {
                Console.WriteLine(responseStream.ResponseStream.Current.Message + " " + responseStream.ResponseStream.Current.Index);
                streamDatas.Add(responseStream.ResponseStream.Current);
            }
            return Ok(JsonSerializer.Serialize(streamDatas));
        }
   
   [HttpGet("[action]")]
        public async Task<IActionResult> GetClientStreamMessage()
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5212");
            var clientStreamMessage = new clientStreamMessage.clientStreamMessageClient(channel);
            using (var call = clientStreamMessage.SayHello())
            {
                for (int i = 0; i < 5; i++)
                {
                   await Task.Delay(1000);
                    Console.WriteLine($"Sending message {i + 1}");
                    await call.RequestStream.WriteAsync(new clientStreamMessageRequest
                    {
                        Name = "Tahir",
                        Index = i
                    });
                }
                await call.RequestStream.CompleteAsync();
                var response = await call.ResponseAsync;
                return Ok(response.Message);
            }
        }   
   
    }
}
