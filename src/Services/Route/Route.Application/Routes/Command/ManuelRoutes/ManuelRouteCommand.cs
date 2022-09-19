using AutoMapper;
using Deliveries;
using Microsoft.EntityFrameworkCore;

namespace Route.Application.Routes.ManuelRoutes;

public class ManuelRouteCommand : IRequest<GenericResponse<ManuelRouteResponse>>
{
    public string CurrentState { get; set; }
    public Guid CorrelationId { get; set; }
}
public class ManuelRouteCommandHandler : IRequestHandler<ManuelRouteCommand, GenericResponse<ManuelRouteResponse>>
{
    private readonly IMessageSender<ICargoApproval> _cargoApproval;
    private readonly IMessageSender<IStartDelivery> _startDelivery;
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;

    public ManuelRouteCommandHandler(IMessageSender<ICargoApproval> cargoApproval, IMessageSender<IStartDelivery> startDelivery, IMapper mapper, IApplicationDbContext context)
    {
        _cargoApproval = cargoApproval;
        _startDelivery = startDelivery;
        _mapper = mapper;
        _context = context;
    }

    public async Task<GenericResponse<ManuelRouteResponse>> Handle(ManuelRouteCommand request, CancellationToken cancellationToken)
    {
        var cargoRoutes = await _context.CargoRoutes.Where(x => x.CorrelationId == request.CorrelationId.ToString()).ToListAsync();
        var routes = _mapper.Map<List<ManuelAutoRouteInstance>>(cargoRoutes);

        var rnd = new Random();
        if (rnd.Next(1, 1000) % 2 == 0)
        {
            await _cargoApproval.SendAsync(new CargoApproval
            {
                CurrentState = request.CurrentState,
                CorrelationId = request.CorrelationId
            }, null, cancellationToken);
        }
        else
        {
            await _startDelivery.SendAsync(new StartDelivery
            {
                CurrentState = request.CurrentState,
                CorrelationId = request.CorrelationId,
                Routes = routes
            }, null, cancellationToken);
        }

        return GenericResponse<ManuelRouteResponse>.Success(new ManuelRouteResponse { }, 200);
    }
}