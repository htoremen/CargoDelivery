using Cargos;
using Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.Application.Routes.ManuelRoutes;

public class ManuelRouteCommand : IRequest<GenericResponse<ManuelRouteResponse>>
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}
public class ManuelRouteCommandHandler : IRequestHandler<ManuelRouteCommand, GenericResponse<ManuelRouteResponse>>
{
    private readonly ISendEndpoint _sendEndpoint;
    private readonly IQueueConfiguration _queueConfiguration;

    public ManuelRouteCommandHandler(ISendEndpointProvider sendEndpointProvider, IQueueConfiguration queueConfiguration)
    {
        _queueConfiguration = queueConfiguration;
        _sendEndpoint = sendEndpointProvider.GetSendEndpoint(new($"queue:{_queueConfiguration.Names[QueueName.CargoSaga]}")).Result;
    }

    public async Task<GenericResponse<ManuelRouteResponse>> Handle(ManuelRouteCommand request, CancellationToken cancellationToken)
    {
        var rnd = new Random();
        if (rnd.Next(1, 1000) % 2 == 0)
        {
            await _sendEndpoint.Send<ICargoApproval>(new
            {
                CargoId = request.CargoId,
                CorrelationId = request.CorrelationId

            }, cancellationToken);
        }
        else
        {
            await _sendEndpoint.Send<IStartDelivery>(new
            {
                CargoId = request.CargoId,
                CorrelationId = request.CorrelationId
            }, cancellationToken);
        }

        return GenericResponse<ManuelRouteResponse>.Success(new ManuelRouteResponse { }, 200);
    }
}