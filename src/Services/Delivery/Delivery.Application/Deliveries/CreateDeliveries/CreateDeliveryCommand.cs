namespace Delivery.Application.Deliveries.CreateDeliveries;

public class CreateDeliveryCommand : IRequest<GenericResponse<CreateDeliveryResponse>>
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}
public class CreateDeliveryCommandHandler : IRequestHandler<CreateDeliveryCommand, GenericResponse<CreateDeliveryResponse>>
{
    public async Task<GenericResponse<CreateDeliveryResponse>> Handle(CreateDeliveryCommand request, CancellationToken cancellationToken)
    {
        return GenericResponse<CreateDeliveryResponse>.Success(new CreateDeliveryResponse { }, 200);
    }
}
