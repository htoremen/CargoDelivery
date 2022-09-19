using AutoMapper;
using Core.Domain.MessageBrokers;
using Core.Domain.SagaInstances;
using Deliveries;
using Microsoft.EntityFrameworkCore;
using Route.Application.Common.Interfaces;

namespace Route.Application.Routes.AutoRoutes;

public class AutoRouteCommand : IRequest<GenericResponse<AutoRouteResponse>>
{
    public string CurrentState { get; set; }
    public Guid CorrelationId { get; set; }
}

public class AutoRouteCommandHandler : IRequestHandler<AutoRouteCommand, GenericResponse<AutoRouteResponse>>
{
    private readonly IMessageSender<IStartDelivery> _startDelivery;
    private readonly IMessageSender<ICargoApproval> _cargoApproval;
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AutoRouteCommandHandler(IMessageSender<IStartDelivery> startDelivery, IMessageSender<ICargoApproval> cargoApproval, IApplicationDbContext context, IMapper mapper)
    {
        _startDelivery = startDelivery;
        _cargoApproval = cargoApproval;
        _context = context;
        _mapper = mapper;
    }

    public async Task<GenericResponse<AutoRouteResponse>> Handle(AutoRouteCommand request, CancellationToken cancellationToken)
    {
        var cargoRoutes = await _context.CargoRoutes.Where(x => x.CorrelationId == request.CorrelationId.ToString()).ToListAsync();
        var routes = _mapper.Map<List<ManuelAutoRouteInstance>>(cargoRoutes);
        
        var rnd = new Random();
        if (rnd.Next(1, 1000) % 2 == 0)
        {
            await _startDelivery.SendAsync(new StartDelivery
            { 
                CurrentState = request.CurrentState,
                CorrelationId = request.CorrelationId,
                Routes = routes
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

        return GenericResponse<AutoRouteResponse>.Success(new AutoRouteResponse { }, 200);
    }
}

