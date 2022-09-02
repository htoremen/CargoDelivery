namespace Delivery.Application.Deliveries.StartDeliveries;

public class StartDeliveryCommand  : IRequest<GenericResponse<StartDeliveryResponse>>
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

public class StartDeliveryCommandHandler : IRequestHandler<StartDeliveryCommand, GenericResponse<StartDeliveryResponse>>
{
    public async Task<GenericResponse<StartDeliveryResponse>> Handle(StartDeliveryCommand request, CancellationToken cancellationToken)
    {
        return GenericResponse<StartDeliveryResponse>.Success(new StartDeliveryResponse { }, 200);
    }
}

