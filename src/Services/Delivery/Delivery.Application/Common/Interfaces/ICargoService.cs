using Cargo.GRPC.Server;

namespace Delivery.Application.Common.Interfaces;

public interface ICargoService
{
    Task<GetCargosResponse> GetCargoAllAsync(string correlationId);
}
