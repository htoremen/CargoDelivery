using Delivery.GRPC.Server;
using Grpc.Net.Client;
using static Delivery.GRPC.Server.DeliveryGrpc;

namespace Payment.GRPC.Client.Services;
public interface IDeliveryService
{
    Task<UpdatePaymentTypeResponse> UpdatePaymentType(UpdatePaymentTypeRequest request);
}

public class DeliveryService : IDeliveryService
{
    private GrpcChannel Channel { get; set; }
    private DeliveryGrpcClient Client { get; set; }
    public DeliveryService()
    {
        Channel = GrpcChannel.ForAddress("https://localhost:5011");
        Client = new DeliveryGrpcClient(Channel);
    }

    public async Task<UpdatePaymentTypeResponse> UpdatePaymentType(UpdatePaymentTypeRequest request)
    {
        var response = await Client.UpdatePaymentTypeAsync(request);
        return response;
    }
}
