using Deliveries;
using MassTransit;

namespace Payment.Application.Payments.FreeDeliveries;

public class FreeDeliveryCommand : IRequest<GenericResponse<FreeDeliveryResponse>>
{
    public string CurrentState { get; set; }
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}
public class FreeDeliveryCommandHandler : IRequestHandler<FreeDeliveryCommand, GenericResponse<FreeDeliveryResponse>>
{
    public async Task<GenericResponse<FreeDeliveryResponse>> Handle(FreeDeliveryCommand request, CancellationToken cancellationToken)
    {
        return GenericResponse<FreeDeliveryResponse>.Success(new FreeDeliveryResponse { }, 200);
    }
}