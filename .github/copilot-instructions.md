# Copilot Instructions for gRPC Solution

## Big Picture Architecture
- This solution contains two main projects: `gRPCServer` and `gRPCClient`, each in its own directory.
- Communication is via gRPC, with `.proto` files in the `Protos/` folder of each project defining service contracts and message types.
- The server implements multiple streaming and unary services (see `Services/`), while the client consumes these services.

## Key Components
- **gRPCServer/Services/**: Contains service implementations. Each service inherits from a generated base class (e.g., `BiDirectionalMessage.BiDirectionalMessageBase`).
- **gRPCClient/Controllers/**: Client-side logic for calling server endpoints.
- **Protos/**: Protocol buffer definitions. Changes here require regeneration of C# classes (see build workflow).

## Developer Workflows
- **Build**: Use Visual Studio or `dotnet build gRPC.sln` from the solution root.
- **Run Server/Client**: Use Visual Studio or `dotnet run --project gRPCServer` and `dotnet run --project gRPCClient`.
- **Regenerate gRPC code after .proto changes**: Rebuild the project; code generation is handled automatically via SDK-style project configuration.
- **Debugging**: Use Visual Studio's debugger or launch with `dotnet run` and attach as needed.

## Project-Specific Patterns
- **Service Implementation**: Each service class overrides methods from the generated base, using async/await and streaming patterns as appropriate.
- **Message Handling**: For streaming, use `IAsyncStreamReader<T>` and/or `IServerStreamWriter<T>` in service methods.
- **Reply Messages**: Return reply objects with clear, user-friendly messages (e.g., `Message = "Message accepted successfully!"`).
- **Cancellation**: Use `CancellationToken` for stream control, but note that new tokens are created per request (see `clientStreamMessageService.cs`).

## Integration Points
- **External Dependencies**: Uses NuGet packages like `Grpc.Net.Client`, `Grpc.Core`, and `Google.Protobuf`.
- **Docker Support**: Both projects include `Dockerfile` for containerization.

## Conventions
- **Naming**: Service classes and proto files use camelCase or PascalCase consistently.
- **Directory Structure**: Keep generated code, service implementations, and proto files organized by project.
- **Error Handling**: Minimal custom error handling; rely on gRPC framework defaults unless otherwise specified.

## Example: Bi-Directional Streaming Service
- See `gRPCServer/Services/BiDirectionalMessageService.cs` for a template of a bi-directional streaming method:
  ```csharp
  public override async Task SayHello(IAsyncStreamReader<BiDirectionalMessageRequest> requestStream, IServerStreamWriter<BiDirectionalMessageReply> responseStream, ServerCallContext context)
  {
      // ...handle incoming stream, send replies...
  }
  ```

## Key Files & Directories
- `gRPC.sln`: Solution file
- `gRPCServer/Services/`: Server-side service implementations
- `gRPCClient/Controllers/`: Client-side logic
- `Protos/`: Protocol buffer definitions
- `Dockerfile`: Containerization setup

---
_If any section is unclear or missing important details, please provide feedback to improve these instructions._
