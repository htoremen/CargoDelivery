using Cargo.GRPC.Server;
using Grpc.Net.Client;
using static Cargo.GRPC.Server.CargoGrpc;

namespace Route.GRPC.Client.Services;
public interface ICargoService
{
    Task<GetCargosResponse> GetCargoAllAsync(string correlationId);
    Task<GetCargoListResponse> GetCargoListAsync(string correlationId);
}

public class CargoService : ICargoService
{
    private GrpcChannel Channel { get; set; }
    private CargoGrpcClient Client { get; set; }

    public CargoService()
    {
        Channel = GrpcChannel.ForAddress("https://localhost:5011");
        Client = new CargoGrpcClient(Channel);
    }

    public async Task<GetCargosResponse> GetCargoAllAsync(string correlationId)
    {
        var response = await Client.GetCargoAllAsync(new GetCargosRequest { CorrelationId = correlationId });
        return response;
    }

    public async Task<GetCargoListResponse> GetCargoListAsync(string correlationId)
    {
        var response = await Client.GetCargoListAsync(new GetCargoListRequest { CorrelationId = correlationId });
        return response;
    }
}