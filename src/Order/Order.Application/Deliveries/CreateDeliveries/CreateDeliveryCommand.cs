using Enums;

namespace Order.Application.Deliveries.CreateDeliveries;

public class CreateDeliveryCommand : IRequest<GenericResponse<CreateDeliveryResponse>>
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
    public PaymentType PaymentType { get; set; }
}
public class CreateDeliveryCommandHandler : IRequestHandler<CreateDeliveryCommand, GenericResponse<CreateDeliveryResponse>>
{
    private readonly IMessageSender<ICreateDelivery> _createDelivery;

    public CreateDeliveryCommandHandler(IMessageSender<ICreateDelivery> createDelivery)
    {
        _createDelivery = createDelivery;
    }

    public async Task<GenericResponse<CreateDeliveryResponse>> Handle(CreateDeliveryCommand request, CancellationToken cancellationToken)
    {
        await _createDelivery.SendAsync(new CreateDelivery
        {
            CargoId = request.CargoId,
            CorrelationId = request.CorrelationId,
            PaymentType = request.PaymentType
        }, null, cancellationToken);
        return GenericResponse<CreateDeliveryResponse>.Success(new CreateDeliveryResponse { CargoId = request.CargoId, CorrelationId = request.CorrelationId }, 200);
    }
}
