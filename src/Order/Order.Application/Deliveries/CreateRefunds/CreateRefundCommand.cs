namespace Order.Application.Deliveries.CreateRefunds;

public class CreateRefundCommand : IRequest<GenericResponse<CreateRefundResponse>>
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

public class CreateRefundCommandHandler : IRequestHandler<CreateRefundCommand, GenericResponse<CreateRefundResponse>>
{
    private readonly IMessageSender<ICreateRefund> _createRefund;

    public CreateRefundCommandHandler(IMessageSender<ICreateRefund> createRefund)
    {
        _createRefund = createRefund;
    }

    public async Task<GenericResponse<CreateRefundResponse>> Handle(CreateRefundCommand request, CancellationToken cancellationToken)
    {
        await _createRefund.SendAsync(new CreateRefund
        {
            CargoId = request.CargoId,
            CorrelationId = request.CorrelationId
        }, null, cancellationToken);
        return GenericResponse<CreateRefundResponse>.Success(new CreateRefundResponse { CargoId = request.CargoId }, 200);

    }
}
