
namespace Delivery.Application.Deliveries.NotDelivereds;

public class NotDeliveredCommand : IRequest<GenericResponse<NotDeliveredResponse>>
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
    public string CurrentState { get; set; }
}

public class NotDeliveredCommandHandler : IRequestHandler<NotDeliveredCommand, GenericResponse<NotDeliveredResponse>>
{
    private readonly IApplicationDbContext _context;

    public NotDeliveredCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GenericResponse<NotDeliveredResponse>> Handle(NotDeliveredCommand request, CancellationToken cancellationToken)
    {
        var cargo = await _context.Cargos.FirstOrDefaultAsync(x => x.CorrelationId == request.CorrelationId.ToString() && x.CargoId == request.CargoId.ToString());
        if(cargo == null)
            return null;

        var deliveryType = (int)DeliveryType.NotDelivered;

        var delivery = await _context.Deliveries.FirstOrDefaultAsync(x => x.CorrelationId == request.CorrelationId.ToString() && x.CargoId == request.CargoId.ToString());
        if(delivery == null)
        {
            delivery = _context.Deliveries.Add(new Domain.Entities.Delivery
            {
                DeliveryId = Guid.NewGuid().ToString(),
                CorrelationId = cargo.CorrelationId,
                CargoId = cargo.CargoId,
                DeliveryType = deliveryType,
                IsCompleted = delivery.IsCompleted,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow
            }).Entity;
            await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            delivery.DeliveryType = deliveryType;
            delivery.EndDate = DateTime.UtcNow;
            _context.Deliveries.Update(delivery);
            await _context.SaveChangesAsync(cancellationToken);
        }
        return GenericResponse<NotDeliveredResponse>.Success(new NotDeliveredResponse { CargoId = request.CargoId, CorrelationId = request.CorrelationId }, 200);
    }
}
