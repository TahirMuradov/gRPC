using Grpc.Core;


namespace gRPCServer.Services
{
    public class BiDirectionalMessageService : BiDirectionalMessage.BiDirectionalMessageBase
    {
        public override async Task SayHello(IAsyncStreamReader<BiDirectionalMessageRequest> requestStream, IServerStreamWriter<BiDirectionalMessageReply> responseStream, ServerCallContext context)
        {
            var task1 = Task.Run(async () =>
              {
                  CancellationToken cancellationToken = new CancellationToken();
                  while (await requestStream.MoveNext(cancellationToken))
                  {
                      await Task.Delay(1000);
                      Console.WriteLine(requestStream.Current.Name + " " + requestStream.Current.Index);
                      BiDirectionalMessageReply biDirectionalMessageReply = new BiDirectionalMessageReply
                      {
                          Message = $"Hello {requestStream.Current.Name}",
                          Index = requestStream.Current.Index
                      };

                      Console.WriteLine($"Send Message to client {biDirectionalMessageReply.Index} " + biDirectionalMessageReply.Message);
                      await responseStream.WriteAsync(biDirectionalMessageReply);
                  }

              });
            await task1;
        }
    }
}
