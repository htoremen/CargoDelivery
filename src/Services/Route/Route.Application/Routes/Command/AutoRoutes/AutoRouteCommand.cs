using AutoMapper;

namespace Route.Application.Routes.AutoRoutes;

public class AutoRouteCommand : IRequest<GenericResponse<AutoRouteResponse>>
{
    public string CurrentState { get; set; }
    public Guid CorrelationId { get; set; }
}

public class AutoRouteCommandHandler : IRequestHandler<AutoRouteCommand, GenericResponse<AutoRouteResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AutoRouteCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GenericResponse<AutoRouteResponse>> Handle(AutoRouteCommand request, CancellationToken cancellationToken)
    {        
        return GenericResponse<AutoRouteResponse>.Success(new AutoRouteResponse { }, 200);
    }
}

