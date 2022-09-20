using Cargo.GRPC.Server;
using Grpc.Net.Client;
using static Cargo.GRPC.Server.DebitGrpc;

namespace Route.Infrastructure.Services;

public class DebitService : IDebitService
{
    private GrpcChannel Channel { get; set; }
    private DebitGrpcClient Client { get; set; }

    public DebitService()
    {
        Channel = GrpcChannel.ForAddress("https://localhost:5011");
        Client = new DebitGrpcClient(Channel);
    }

    public async Task<StateUpdateResponse> UpdateStateAsync(string correlationId, string currentState)
    {
        var response = await Client.UpdateStateAsync(new StateUpdateRequest { CorrelationId = correlationId, CurrentState = currentState });
        return response;
    }
}
