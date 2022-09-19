using Cargo.GRPC.Server;
using Grpc.Net.Client;

namespace Delivery.Application.Cargos.Queries.GetCargoAlls;

public class GetCargoAllQuery : IRequest<GenericResponse<GetCargosResponse>>
{
    public string CorrelationId { get; set; }
}

public class GetCargoAllQueryHandler : IRequestHandler<GetCargoAllQuery, GenericResponse<GetCargosResponse>>
{
    public async Task<GenericResponse<GetCargosResponse>> Handle(GetCargoAllQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5011");
            var client = new CargoGrpc.CargoGrpcClient(channel);

            var response = await client.GetCargoAllAsync(new GetCargosRequest { CorrelationId = request.CorrelationId });

            return GenericResponse<GetCargosResponse>.Success(response, 200);
        }
        catch (Exception ex)
        {

        }
        return GenericResponse<GetCargosResponse>.Success(200);
    }
}

