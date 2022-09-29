using Cargo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cargo.Application.Cargos.Commands.CreateCargos;

public class CreateCargoCommand : IRequest<GenericResponse<CreateCargoResponse>>
{
    public Guid CorrelationId { get; set; }
    public Guid DebitId { get; set; }
    public Guid CargoId { get; set; }
    public string Address { get; set; }
    public List<CreateCargoCargoItem> CargoItems { get; set; }
}

public class CreateCargoCommandHandler : IRequestHandler<CreateCargoCommand, GenericResponse<CreateCargoResponse>>
{
    private readonly IApplicationDbContext _context;

    public CreateCargoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GenericResponse<CreateCargoResponse>> Handle(CreateCargoCommand request, CancellationToken cancellationToken)
    {
        var cargo = await _context.Cargos.FirstOrDefaultAsync(x => x.Debit.CorrelationId == request.CorrelationId.ToString() && 
                                                                   x.DebitId == request.DebitId.ToString() && 
                                                                   x.CargoId == request.CargoId.ToString());
        if (cargo == null)
        {
            cargo = _context.Cargos.Add(new Domain.Entities.Cargo
            {
                DebitId = request.DebitId.ToString(),
                CargoId = request.CargoId.ToString(),
                Address = request.Address
            }).Entity;
        }
        var cargoItems = new List<CargoItem>();
        foreach (var cargoItem in request.CargoItems)
        {
            var cargoItemCheck = await _context.CargoItems.FirstOrDefaultAsync(x => x.Cargo.Debit.CorrelationId == request.CorrelationId.ToString() &&
                                                                   x.Cargo.DebitId == request.DebitId.ToString() &&
                                                                   x.CargoId == request.CargoId.ToString() &&
                                                                   x.CargoItemId == cargoItem.CargoItemId.ToString());
            if (cargoItemCheck == null)
            {
                _context.CargoItems.Add(new CargoItem
                {
                    CargoId = cargo.CargoId,
                    CargoItemId = cargoItem.CargoItemId.ToString(),
                    Barcode = cargoItem.Barcode,
                    Description = cargoItem.Description,
                    Desi = cargoItem.Desi,
                    Kg = cargoItem.Kg,
                    WaybillNumber = cargoItem.WaybillNumber,
                });
            }
        }
        await _context.SaveChangesAsync(cancellationToken);

        return GenericResponse<CreateCargoResponse>.Success(new CreateCargoResponse { CorrelationId = request.CorrelationId, CargoId = request.CargoId.ToString() }, 200);
    }
}

