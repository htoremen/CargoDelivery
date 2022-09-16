namespace Delivery.Application.Deliveries.StartDeliveries;

public class StartDeliveryCommand  : IRequest<GenericResponse<StartDeliveryResponse>>
{
    public string CurrentState { get; set; }
    public Guid CorrelationId { get; set; }
}

public class StartDeliveryCommandHandler : IRequestHandler<StartDeliveryCommand, GenericResponse<StartDeliveryResponse>>
{
    public async Task<GenericResponse<StartDeliveryResponse>> Handle(StartDeliveryCommand request, CancellationToken cancellationToken)
    {
        return GenericResponse<StartDeliveryResponse>.Success(new StartDeliveryResponse { }, 200);
    }
}

