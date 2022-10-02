using Cargo.GRPC.Server;
using Core.Infrastructure.Grpc;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Configuration;
using Microsoft.AspNetCore.Builder;
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

    /// <summary>
    /// https://binodmahto.medium.com/best-practices-with-grpc-on-net-38b215b0a698
    /// </summary>
    public CargoService()
    {
        Channel = GrpcChannel.ForAddress("https://localhost:5011", GrpcExtensions.GetGrpcChannelOptions());
        Client = new CargoGrpcClient(Channel);
    }

    public async Task<GetCargosResponse> GetCargoAllAsync(string correlationId)
    {
        try
        {
            var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            var response = await Client.GetCargoAllAsync(new GetCargosRequest { CorrelationId = correlationId },
                                                        headers: null, 
                                                        deadline: DateTime.UtcNow.AddSeconds(5), 
                                                        cancellationToken.Token);

            return response;
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled || ex.StatusCode == StatusCode.DeadlineExceeded)
        {
            // Stream cancelled/timeout.
            return new GetCargosResponse();
        }
    }

    public async Task<GetCargoListResponse> GetCargoListAsync(string correlationId)
    {
        var response = await Client.GetCargoListAsync(new GetCargoListRequest { CorrelationId = correlationId });
        return response;
    }
}