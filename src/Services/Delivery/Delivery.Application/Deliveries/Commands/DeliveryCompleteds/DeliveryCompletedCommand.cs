namespace Delivery.Application.Deliveries.DeliveryCompleteds;

public class DeliveryCompletedCommand : IRequest<GenericResponse<DeliveryCompletedResponse>>
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}

public class DeliveryCompletedCommandHandler : IRequestHandler<DeliveryCompletedCommand, GenericResponse<DeliveryCompletedResponse>>
{
    private readonly IApplicationDbContext _context;

    public DeliveryCompletedCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GenericResponse<DeliveryCompletedResponse>> Handle(DeliveryCompletedCommand request, CancellationToken cancellationToken)
    {
        var isDeliveryCompleted = await _context.Cargos.AnyAsync(x => !x.Deliveries.Any());
        return GenericResponse<DeliveryCompletedResponse>.Success(new DeliveryCompletedResponse { IsDeliveryCompleted = isDeliveryCompleted }, 200);
    }
}