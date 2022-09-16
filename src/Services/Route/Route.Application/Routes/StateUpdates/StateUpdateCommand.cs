using Cargo.GRPC;
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
        var client = DebitGrpc.DebitGrpcClient(channel);
        

        return GenericResponse<StateUpdateResponse>.Success(200);
    }
}