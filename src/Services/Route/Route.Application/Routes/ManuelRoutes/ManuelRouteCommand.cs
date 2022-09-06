using Core.Domain.MessageBrokers;
using Deliveries;

namespace Route.Application.Routes.ManuelRoutes;

public class ManuelRouteCommand : IRequest<GenericResponse<ManuelRouteResponse>>
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}
public class ManuelRouteCommandHandler : IRequestHandler<ManuelRouteCommand, GenericResponse<ManuelRouteResponse>>
{
    private readonly IMessageSender<ICargoApproval> _cargoApproval;
    private readonly IMessageSender<IStartDelivery> _startDelivery;

    public ManuelRouteCommandHandler(IMessageSender<ICargoApproval> cargoApproval, IMessageSender<IStartDelivery> startDelivery)
    {
        _cargoApproval = cargoApproval;
        _startDelivery = startDelivery;
    }

    public async Task<GenericResponse<ManuelRouteResponse>> Handle(ManuelRouteCommand request, CancellationToken cancellationToken)
    {
        var rnd = new Random();
        if (rnd.Next(1, 1000) % 2 == 0)
        {
            await _cargoApproval.SendAsync(new CargoApproval
            {
                CargoId = request.CargoId,
                CorrelationId = request.CorrelationId

            }, null, cancellationToken);
        }
        else
        {
            await _startDelivery.SendAsync(new StartDelivery
            {
                CargoId = request.CargoId,
                CorrelationId = request.CorrelationId
            }, null, cancellationToken);
        }

        return GenericResponse<ManuelRouteResponse>.Success(new ManuelRouteResponse { }, 200);
    }
}