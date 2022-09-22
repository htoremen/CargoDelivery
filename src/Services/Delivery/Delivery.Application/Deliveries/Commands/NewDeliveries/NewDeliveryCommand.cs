namespace Delivery.Application.Deliveries.Commands.NewDeliveries;

public class NewDeliveryCommand : IRequest<GenericResponse<NewDeliveryResponse>>
{
    public string CorrelationId { get; set; }
    public string CurrentState { get; set; }
}


public class NewDeliveryCommandHandler : IRequestHandler<NewDeliveryCommand, GenericResponse<NewDeliveryResponse>>
{
    private readonly IDebitService _debitService;

    public NewDeliveryCommandHandler(IDebitService debitService)
    {
        _debitService = debitService;
    }

    public async Task<GenericResponse<NewDeliveryResponse>> Handle(NewDeliveryCommand request, CancellationToken cancellationToken)
    {
        await _debitService.UpdateStateAsync(request.CorrelationId, request.CurrentState);

        return GenericResponse<NewDeliveryResponse>.Success(200);
    }
}
