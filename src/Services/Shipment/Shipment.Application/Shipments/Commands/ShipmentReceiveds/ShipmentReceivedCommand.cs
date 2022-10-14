using Microsoft.EntityFrameworkCore;
using Shipment.Application.Common.Interfaces;

namespace Shipment.Application.Shipments.Commands.ShipmentReceiveds;

public class ShipmentReceivedCommand : IRequest<GenericResponse<ShipmentReceivedResponse>>
{
    public string CorrelationId { get; set; }
    public string DebitId { get; set; }
    public string CargoId { get; set; }
    public string CurrentState { get; set; }
    public int ShipmentTypeId { get; set; }
}

public class ShipmentReceivedCommandHandler : IRequestHandler<ShipmentReceivedCommand, GenericResponse<ShipmentReceivedResponse>>
{
    private readonly IApplicationDbContext _context;

    public ShipmentReceivedCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GenericResponse<ShipmentReceivedResponse>> Handle(ShipmentReceivedCommand request, CancellationToken cancellationToken)
    {
        var cargo = await _context.Cargos.FirstOrDefaultAsync(x => x.DebitId == request.DebitId.ToString() && x.CargoId == request.CargoId.ToString());
        if(cargo == null)
        {
            _context.Cargos.Add(new Domain.Entities.Cargo
            {
                CargoId = request.CargoId,
                DebitId = request.DebitId,
                CreateDate = DateTime.UtcNow,
                ShipmentTypeId = request.ShipmentTypeId
            })
        }
        return GenericResponse<ShipmentReceivedResponse>.Success(200);
    }
}
