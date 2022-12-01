

namespace Payment.Application.Payments.PayAtDoors;

public class PayAtDoorCommand : IRequest<GenericResponse<PayAtDoorResponse>>
{
    public string CurrentState { get; set; }
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}

public class PayAtDoorCommandHandler : IRequestHandler<PayAtDoorCommand, GenericResponse<PayAtDoorResponse>>
{
    public async Task<GenericResponse<PayAtDoorResponse>> Handle(PayAtDoorCommand request, CancellationToken cancellationToken)
    {
        return GenericResponse<PayAtDoorResponse>.Success(new PayAtDoorResponse { }, 200);
    }
}
