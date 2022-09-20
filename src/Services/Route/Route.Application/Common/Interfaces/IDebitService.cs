using Cargo.GRPC.Server;

namespace Route.Application.Common.Interfaces;

public interface IDebitService
{
    Task<StateUpdateResponse> UpdateStateAsync(string correlationId, string currentState);
}
