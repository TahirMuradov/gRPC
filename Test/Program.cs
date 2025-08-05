using Grpc.Net.Client;
using gRPCServer;

var channel = GrpcChannel.ForAddress("http://localhost:5212");
var biDirectionalMessage = new BiDirectionalMessage.BiDirectionalMessageClient(channel);
CancellationToken cancellationToken = new CancellationToken();
var task1 = Task.Run(async () =>
{
    using (var call = biDirectionalMessage.SayHello())
    {
        for (int i = 0; i < 5; i++)
        {
            await Task.Delay(1000);
            Console.WriteLine($"Sending message to server {i + 1}");
            await call.RequestStream.WriteAsync(new BiDirectionalMessageRequest
            {
                Name = "Tahir",
                Index = i
            });
            if (await call.ResponseStream.MoveNext(cancellationToken))
            {
                await Task.Delay(1000);

                Console.WriteLine("from Server " + call.ResponseStream.Current.Message + " " + call.ResponseStream.Current.Index);


            }
        }
        await call.RequestStream.CompleteAsync();

    }

});
await task1;