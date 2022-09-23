using Delivery.Application.Deliveries.Commands.UpdatePaymentTypes;

namespace Delivery.GRPC.Server.Services;

public class DeliveryService : DeliveryGrpc.DeliveryGrpcBase
{
    private readonly IMediator _mediator;

    public DeliveryService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<UpdatePamentTypeResponse> UpdatePaymentType(UpdatePamentTypeRequest request, ServerCallContext context)
    {
        await _mediator.Send(new UpdatePaymentTypeCommand
        {
            CorrelationId = request.CorrelationId,
            CargoId = request.CargoId,
            PaymentType = request.PaymentType
        });
        var response = new UpdatePamentTypeResponse();
        return response;
    }
}
