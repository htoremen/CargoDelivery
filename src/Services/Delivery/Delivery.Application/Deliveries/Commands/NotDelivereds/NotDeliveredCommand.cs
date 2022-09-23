
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
        return GenericResponse<NotDeliveredResponse>.Success(new NotDeliveredResponse { CargoId = request.CargoId, CorrelationId = request.CorrelationId }, 200);
    }
}
