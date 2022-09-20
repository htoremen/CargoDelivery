using Cargo.GRPC.Server;
using Grpc.Net.Client;
using static Cargo.GRPC.Server.CargoGrpc;

namespace Delivery.Infrastructure.Services;

public class CargoService : ICargoService
{
    private GrpcChannel Channel { get; set; }
    private CargoGrpcClient Client { get; set; }

    public CargoService()
    {
        Channel = GrpcChannel.ForAddress("https://localhost:5011");
        Client = new CargoGrpc.CargoGrpcClient(Channel);
    }

    public async Task<GetCargosResponse> GetCargoAllAsync(string correlationId)
    {
        var response = await Client.GetCargoAllAsync(new GetCargosRequest { CorrelationId = correlationId });
        return response;
    }
}
