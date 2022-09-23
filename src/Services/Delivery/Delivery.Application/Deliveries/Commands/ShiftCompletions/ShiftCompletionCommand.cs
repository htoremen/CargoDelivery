
using MassTransit;

namespace Delivery.Application.Deliveries.ShiftCompletions;

public class ShiftCompletionCommand : IRequest<GenericResponse<ShiftCompletionResponse>>
{
    public Guid CorrelationId { get; set; }
}


public class ShiftCompletionCommandHandler : IRequestHandler<ShiftCompletionCommand, GenericResponse<ShiftCompletionResponse>>
{
    private readonly IMessageSender<IShiftCompletion> _shiftCompletion;

    public ShiftCompletionCommandHandler(IMessageSender<IShiftCompletion> shiftCompletion)
    {
        _shiftCompletion = shiftCompletion;
    }

    public async Task<GenericResponse<ShiftCompletionResponse>> Handle(ShiftCompletionCommand request, CancellationToken cancellationToken)
    {
        await _shiftCompletion.SendAsync(new ShiftCompletion
        {
            CorrelationId = request.CorrelationId
        }, null, cancellationToken);
        return GenericResponse<ShiftCompletionResponse>.Success(new ShiftCompletionResponse { CorrelationId = request.CorrelationId }, 200);
    }
}

