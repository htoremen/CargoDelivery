using Core.Domain.Enums;

namespace Delivery.Application.Deliveries.DeliveryCompleteds;

public class DeliveryCompletedCommand : IRequest<GenericResponse<DeliveryCompletedResponse>>
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}

public class DeliveryCompletedCommandHandler : IRequestHandler<DeliveryCompletedCommand, GenericResponse<DeliveryCompletedResponse>>
{
    private readonly IMessageSender<IStartDelivery> _startDelivery;

    public DeliveryCompletedCommandHandler(IMessageSender<IStartDelivery> startDelivery)
    {
        _startDelivery = startDelivery;
    }

    public async Task<GenericResponse<DeliveryCompletedResponse>> Handle(DeliveryCompletedCommand request, CancellationToken cancellationToken)
    {
        var rnd = new Random();
        if (rnd.Next(1, 1000) % 2 == 0)
        {
            await _startDelivery.SendAsync(new StartDelivery
            {
                CargoId = request.CargoId,
                CorrelationId = request.CorrelationId

            }, null, cancellationToken);
        }
        return GenericResponse<DeliveryCompletedResponse>.Success(new DeliveryCompletedResponse { }, 200);
    }
}
