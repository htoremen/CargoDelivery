using Delivery.GRPC.Server;
using Grpc.Net.Client;
using static Delivery.GRPC.Server.DeliveryGrpc;

namespace Payment.GRPC.Client.Services;
public interface IDeliveryService
{
    Task<UpdatePaymentTypeResponse> UpdatePaymentType(string correlationId, string cargoId, int paymentType);
}

public class DeliveryService : IDeliveryService
{
    private GrpcChannel Channel { get; set; }
    private DeliveryGrpcClient Client { get; set; }
    public DeliveryService()
    {
        Channel = GrpcChannel.ForAddress("https://localhost:5013");
        Client = new DeliveryGrpcClient(Channel);
    }

    public async Task<UpdatePaymentTypeResponse> UpdatePaymentType(string correlationId, string cargoId, int paymentType)
    {
        var response = await Client.UpdatePaymentTypeAsync(new UpdatePaymentTypeRequest
        {
            CorrelationId = correlationId,
            CargoId = cargoId,
            PaymentType = paymentType
        });
        return response;
    }
}
