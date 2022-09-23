namespace Delivery.Application.Deliveries.DeliveryCompleteds;

public class DeliveryCompletedCommand : IRequest<GenericResponse<DeliveryCompletedResponse>>
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}

public class DeliveryCompletedCommandHandler : IRequestHandler<DeliveryCompletedCommand, GenericResponse<DeliveryCompletedResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IDebitService _debitService;

    public DeliveryCompletedCommandHandler(IApplicationDbContext context, IDebitService debitService)
    {
        _context = context;
        _debitService = debitService;
    }

    public async Task<GenericResponse<DeliveryCompletedResponse>> Handle(DeliveryCompletedCommand request, CancellationToken cancellationToken)
    {
        await _debitService.UpdateStateAsync(request.CorrelationId.ToString(), request.CurrentState);

        var isDeliveryCompleted = await _context.Cargos.AnyAsync(x => x.CorrelationId == request.CorrelationId.ToString() && x.IsCompleted == null);
        return GenericResponse<DeliveryCompletedResponse>.Success(new DeliveryCompletedResponse { IsDeliveryCompleted = !isDeliveryCompleted }, 200);
    }
}