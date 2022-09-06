using Core.Domain.MessageBrokers;
using Deliveries;

namespace Route.Application.Routes.AutoRoutes;

public class AutoRouteCommand : IRequest<GenericResponse<AutoRouteResponse>>
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

public class AutoRouteCommandHandler : IRequestHandler<AutoRouteCommand, GenericResponse<AutoRouteResponse>>
{
    private readonly IMessageSender<IStartDelivery> _startDelivery;
    private readonly IMessageSender<ICargoApproval> _cargoApproval;

    public AutoRouteCommandHandler(IMessageSender<IStartDelivery> startDelivery, IMessageSender<ICargoApproval> cargoApproval)
    {
        _startDelivery = startDelivery;
        _cargoApproval = cargoApproval;
    }

    public async Task<GenericResponse<AutoRouteResponse>> Handle(AutoRouteCommand request, CancellationToken cancellationToken)
    {
        var rnd = new Random();
        if (rnd.Next(1, 1000) % 2 == 0)
        {
            //await _sendEndpoint.Send<ICargoApproval>(new
            //{
            //    CargoId = request.CargoId,
            //    CorrelationId = request.CorrelationId

            //}, cancellationToken);
            await _startDelivery.SendAsync(new StartDelivery
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

        return GenericResponse<AutoRouteResponse>.Success(new AutoRouteResponse { }, 200);
    }
}

