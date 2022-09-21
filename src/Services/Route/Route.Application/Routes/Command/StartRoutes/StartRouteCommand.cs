using Cargo.GRPC.Server;
using Microsoft.EntityFrameworkCore;
using Route.Domain.Entities;

namespace Route.Application.Routes.StartRoutes;

public class StartRouteCommand : IRequest<GenericResponse<StartRouteResponse>>
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public GetCargoListResponse CargoList { get; set; }
}

public class StartRouteCommandCommandHandler : IRequestHandler<StartRouteCommand, GenericResponse<StartRouteResponse>>
{
    private readonly IApplicationDbContext _context;

    public StartRouteCommandCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GenericResponse<StartRouteResponse>> Handle(StartRouteCommand request, CancellationToken cancellationToken)
    {
        foreach (var item in request.CargoList.Cargos)
        {
            var cargoRoute = await _context.CargoRoutes.FirstOrDefaultAsync(x => x.CargoId == item.CargoId.ToString() && x.CorrelationId == request.CorrelationId.ToString());
            if (cargoRoute == null)
            {
                _context.CargoRoutes.Add(new CargoRoute
                {
                    CargoRouteId = Guid.NewGuid().ToString(),
                    Route = "",
                    CargoId = item.CargoId.ToString(),
                    CorrelationId = request.CorrelationId.ToString(),
                    Address = item.Address,
                    CreatedOn = DateTime.Now,
                    IsRoute = true,
                });
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        return GenericResponse<StartRouteResponse>.Success(new StartRouteResponse { CorrelationId = request.CorrelationId }, 200);
    }
}