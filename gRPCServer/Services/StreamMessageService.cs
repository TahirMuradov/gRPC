using Grpc.Core;


namespace gRPCServer.Services
{
    public class StreamMessageService : StreamMessage.StreamMessageBase
    {

        public override async Task SayHello(MessageRequest request, IServerStreamWriter<MessageReply> responseStream, ServerCallContext context)
        {

            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(2000);
                MessageReply message = new MessageReply
                {
                    Index = i,
                    Message = request.Name
                };
                await responseStream.WriteAsync(message);
                Console.WriteLine("Send Message to client "+message);
            }


        }

    }
}
