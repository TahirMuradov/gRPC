using Grpc.Core;


namespace gRPCServer.Services
{
    public class clientStreamMessageService : clientStreamMessage.clientStreamMessageBase
    {
        public override async Task<clientStreamMessageReply> SayHello(IAsyncStreamReader<clientStreamMessageRequest> requestStream, ServerCallContext context)
        {
            CancellationToken cancellationToken = new CancellationToken();

            while (await requestStream.MoveNext(cancellationToken))
            {
                Console.WriteLine(requestStream.Current.Name + " " + requestStream.Current.Index);

            }
            return new clientStreamMessageReply
            {

                Message = "Message accepted successfully!"

            };
        }

    }
}
