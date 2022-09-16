using Cargo.Application.Cargos.Commands.UpdateStates;
using Grpc.Core;
using MediatR;

namespace Cargo.GRPC.Services;

public class DebitService : DebitGrpc.DebitGrpcBase
{
    private readonly IMediator _mediator;

    public DebitService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<StateUpdateResponse> UpdateState(StateUpdateRequest request, ServerCallContext context)
    {
        await _mediator.Send(new UpdateStateCommand
        {
            CorrelationId = request.CorrelationId,
            CurrentState = request.CurrentState,
        });
        var response = new StateUpdateResponse();
        return response;
    }
}
