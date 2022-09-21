using Cargo.GRPC.Server;

namespace Delivery.Application.Cargos.Queries.GetCargoAlls;

public class GetCargoAllQuery : IRequest<GenericResponse<GetCargosResponse>>
{
    public string CorrelationId { get; set; }
}

public class GetCargoAllQueryHandler : IRequestHandler<GetCargoAllQuery, GenericResponse<GetCargosResponse>>
{
    private readonly ICargoService _cargoService;

    public GetCargoAllQueryHandler(ICargoService cargoService)
    {
        _cargoService = cargoService;
    }

    public async Task<GenericResponse<GetCargosResponse>> Handle(GetCargoAllQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _cargoService.GetCargoAllAsync(request.CorrelationId);
            return GenericResponse<GetCargosResponse>.Success(response, 200);
        }
        catch (Exception ex)
        {

        }
        return GenericResponse<GetCargosResponse>.Success(200);
    }
}