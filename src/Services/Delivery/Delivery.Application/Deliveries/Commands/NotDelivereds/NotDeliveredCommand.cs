﻿
namespace Delivery.Application.Deliveries.NotDelivereds;

public class NotDeliveredCommand : IRequest<GenericResponse<NotDeliveredResponse>>
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
    public string CurrentState { get; set; }
    public DeliveryType DeliveryType { get; set; }
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
        var cargo = await _context.Cargos.FirstOrDefaultAsync(x => x.CorrelationId == request.CorrelationId.ToString().ToLower() && x.CargoId == request.CargoId.ToString());
        if(cargo == null)
            return null;

        var deliveryType = (int)request.DeliveryType;

        var delivery = await _context.Deliveries.FirstOrDefaultAsync(x => x.CorrelationId == request.CorrelationId.ToString() && x.CargoId == request.CargoId.ToString());
        if(delivery == null)
        {
            delivery = _context.Deliveries.Add(new Domain.Entities.Delivery
            {
                DeliveryId = Guid.NewGuid().ToString(),
                CorrelationId = cargo.CorrelationId,
                CargoId = cargo.CargoId,
                DeliveryType = deliveryType,
                IsCompleted = true,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow
            }).Entity;

            cargo.IsCompleted = true;
            _context.Cargos.Update(cargo);
            await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            delivery.DeliveryType = deliveryType;
            delivery.EndDate = DateTime.UtcNow;
            _context.Deliveries.Update(delivery);

            cargo.IsCompleted = true;
            _context.Cargos.Update(cargo);

            await _context.SaveChangesAsync(cancellationToken);
        }
        return GenericResponse<NotDeliveredResponse>.Success(new NotDeliveredResponse { CargoId = request.CargoId, CorrelationId = request.CorrelationId }, 200);
    }
}
