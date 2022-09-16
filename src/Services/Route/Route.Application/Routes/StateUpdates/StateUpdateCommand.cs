using Cargo.GRPC.Server;
using Grpc.Net.Client;

namespace Route.Application.Routes.StateUpdates;

public class StateUpdateCommand : IRequest<GenericResponse<StateUpdateResponse>>
{
    public string CorrelationId { get; set; }
    public string CurrentState { get; set; }
}

public class StateUpdateCommandHandler : IRequestHandler<StateUpdateCommand, GenericResponse<StateUpdateResponse>>
{
    public async Task<GenericResponse<StateUpdateResponse>> Handle(StateUpdateCommand request, CancellationToken cancellationToken)
    {
        var channel = GrpcChannel.ForAddress("https://localhost:5011");
        var client = new DebitGrpc.DebitGrpcClient(channel);

        await client.UpdateStateAsync(new StateUpdateRequest { CorrelationId = request.CorrelationId, CurrentState = request.CurrentState });      

        return GenericResponse<StateUpdateResponse>.Success(200);
    }
}