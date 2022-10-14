namespace Shipment.Application.Shipments.Commands.ShipmentReceiveds;

public class ShipmentReceivedCommand : IRequest<GenericResponse<ShipmentReceivedResponse>>
{
    public Guid CorrelationId { get; set; }
    public Guid DebitId { get; set; }
    public Guid CargoId { get; set; }
    public string CurrentState { get; set; }
}

public class ShipmentReceivedCommandHandler : IRequestHandler<ShipmentReceivedCommand, GenericResponse<ShipmentReceivedResponse>>
{
    public async Task<GenericResponse<ShipmentReceivedResponse>> Handle(ShipmentReceivedCommand request, CancellationToken cancellationToken)
    {
        return GenericResponse<ShipmentReceivedResponse>.Success(200);
    }
}
