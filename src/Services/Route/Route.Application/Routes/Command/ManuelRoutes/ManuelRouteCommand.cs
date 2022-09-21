using AutoMapper;
using Deliveries;

namespace Route.Application.Routes.ManuelRoutes;

public class ManuelRouteCommand : IRequest<GenericResponse<ManuelRouteResponse>>
{
    public string CurrentState { get; set; }
    public Guid CorrelationId { get; set; }
}
public class ManuelRouteCommandHandler : IRequestHandler<ManuelRouteCommand, GenericResponse<ManuelRouteResponse>>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;

    public ManuelRouteCommandHandler(IMapper mapper, IApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GenericResponse<ManuelRouteResponse>> Handle(ManuelRouteCommand request, CancellationToken cancellationToken)
    {
        return GenericResponse<ManuelRouteResponse>.Success(new ManuelRouteResponse { }, 200);
    }
}