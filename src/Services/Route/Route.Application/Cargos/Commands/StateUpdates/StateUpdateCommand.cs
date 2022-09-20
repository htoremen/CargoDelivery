namespace Route.Application.Routes.StateUpdates;

public class StateUpdateCommand : IRequest<GenericResponse<StateUpdateResponse>>
{
    public string CorrelationId { get; set; }
    public string CurrentState { get; set; }
}

public class StateUpdateCommandHandler : IRequestHandler<StateUpdateCommand, GenericResponse<StateUpdateResponse>>
{
    private readonly IDebitService _debitService;

    public StateUpdateCommandHandler(IDebitService debitService)
    {
        _debitService = debitService;
    }

    public async Task<GenericResponse<StateUpdateResponse>> Handle(StateUpdateCommand request, CancellationToken cancellationToken)
    {
        await _debitService.UpdateStateAsync(request.CurrentState, request.CorrelationId);

        return GenericResponse<StateUpdateResponse>.Success(200);
    }
}