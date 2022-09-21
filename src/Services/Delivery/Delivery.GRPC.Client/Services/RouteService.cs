using Grpc.Net.Client;
using Route.GRPC.Server;
using static Route.GRPC.Server.RouteGrpc;

namespace Delivery.GRPC.Client.Services;

public interface IRouteService
{
    Task<GetRouteResponse> GetRouteAsync(string correlationId);
}

public class RouteService : IRouteService
{
    private GrpcChannel Channel { get; set; }
    private RouteGrpcClient Client { get; set; }

    public RouteService()
    {
        Channel = GrpcChannel.ForAddress("https://localhost:5012");
        Client = new RouteGrpcClient(Channel);
    }

    public async Task<GetRouteResponse> GetRouteAsync(string correlationId)
    {
        var response = await Client.GetRouteAsync(new GetRouteRequest { CorrelationId = correlationId });
        return response;    
    }
}