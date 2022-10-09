namespace Order.Application.Deliveries.NotDelivereds;

public class NotDeliveredCommand : IRequest<GenericResponse<NotDeliveredResponse>>
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

public class NotDeliveredCommandHandler : IRequestHandler<NotDeliveredCommand, GenericResponse<NotDeliveredResponse>>
{
    private readonly IMessageSender<INotDelivered> _notDelivered;

    public NotDeliveredCommandHandler(IMessageSender<INotDelivered> notDelivered)
    {
        _notDelivered = notDelivered;
    }

    public async Task<GenericResponse<NotDeliveredResponse>> Handle(NotDeliveredCommand request, CancellationToken cancellationToken)
    {
        await _notDelivered.SendAsync(new NotDelivered
        {
            CargoId = request.CargoId,
            CorrelationId = request.CorrelationId
        }, null, cancellationToken);
        return GenericResponse<NotDeliveredResponse>.Success(new NotDeliveredResponse { CargoId = request.CargoId }, 200);

    }
}
