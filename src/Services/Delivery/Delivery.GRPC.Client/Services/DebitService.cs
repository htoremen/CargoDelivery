using static Cargo.GRPC.Server.DebitGrpc;

namespace Delivery.GRPC.Client.Services;

public interface IDebitService
{
    Task<StateUpdateResponse> UpdateStateAsync(string correlationId, string currentState);
}


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
