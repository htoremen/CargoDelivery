namespace Delivery.Application.Deliveries.Commands.UpdatePaymentTypes;

public class UpdatePaymentTypeCommand : IRequest<GenericResponse<UpdatePaymentTypeResponse>>
{
    public string CorrelationId { get; set; }
    public string CargoId { get; set; }
    public int? PaymentType { get; set; }
}

public class UpdatePaymentTypeCommandhandler : IRequestHandler<UpdatePaymentTypeCommand, GenericResponse<UpdatePaymentTypeResponse>>
{
    private readonly IApplicationDbContext _context;

    public UpdatePaymentTypeCommandhandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GenericResponse<UpdatePaymentTypeResponse>> Handle(UpdatePaymentTypeCommand request, CancellationToken cancellationToken)
    {
        var delivery = await _context.Deliveries.FirstOrDefaultAsync(x => x.CorrelationId == request.CorrelationId && x.CargoId == request.CargoId);
        if (delivery != null)
        {
            delivery.EndDate = DateTime.Now;
            delivery.PaymentType = request.PaymentType;
            _context.Deliveries.Update(delivery);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return GenericResponse<UpdatePaymentTypeResponse>.Success(200);
    }
}
