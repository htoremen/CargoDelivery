using Microsoft.EntityFrameworkCore;

namespace Cargo.Application.Cargos.CreateCargos;

public class CreateCargoCommand : IRequest<GenericResponse<CreateCargoResponse>>
{
    public Guid CorrelationId { get; set; }
    public Guid DebitId { get; set; }
    public Guid CourierId { get; set; }
    public List<CargoDetay> Cargos { get; set; }
}

public class CreateCargoCommandHandler : IRequestHandler<CreateCargoCommand, GenericResponse<CreateCargoResponse>>
{
    private IApplicationDbContext _context;

    public CreateCargoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GenericResponse<CreateCargoResponse>> Handle(CreateCargoCommand request, CancellationToken cancellationToken)
    {
        var debit = await _context.Debits.FirstOrDefaultAsync(x => x.DistributionDate != DateTime.Now && x.CourierId == request.CourierId.ToString());
        if (debit == null)
        {
            var cargos = new List<Domain.Entities.Cargo>();
            foreach (var cargo in request.Cargos)
            {
                var cargoItems = new List<Domain.Entities.CargoItem>();
                foreach (var cargoItem in cargo.CargoItems)
                {
                    cargoItems.Add(new CargoItem
                    {
                        CargoItemId = Guid.NewGuid().ToString(),
                        Address = cargoItem.Address,
                        Barcode = cargoItem.Barcode,
                        Description = cargoItem.Description,
                        Desi = cargoItem.Desi,
                        Kg = cargoItem.Kg,
                        WaybillNumber = cargoItem.WaybillNumber,
                    });
                }
                cargos.Add(new Domain.Entities.Cargo
                {
                    CargoId = Guid.NewGuid().ToString(),
                    Address = cargo.Address,
                    CargoItems = cargoItems
                });
            }

            debit = _context.Debits.Add(new Debit
            {
                DebitId = request.DebitId.ToString(),
                CorrelationId = request.CorrelationId.ToString(),
                CourierId = request.CourierId.ToString(),
                DistributionDate = DateTime.Now.Date,
                StartingDate = DateTime.Now,
                IsApproval = false,
                IsCompleted = false,
                Cargos = cargos
            }).Entity;
            await _context.SaveChangesAsync(cancellationToken);
        }

       

        //var cargo = _context.Cargos.Add(new Domain.Entities.Cargo
        //{
        //    Address = request.Cargos.Address,
        //    DebitId = debit.DebitId
        //}).Entity;

        //foreach (var item in request.Cargos.CargoItems)
        //{
        //    var cargoItem = await _context.CargoItems.FirstOrDefaultAsync(x => x.Cargo.DebitId == debit.DebitId);
        //    if (cargoItem == null)
        //    {

        //    }
        //}


        return GenericResponse<CreateCargoResponse>.Success(new CreateCargoResponse { DebitId = Guid.Parse(debit.DebitId), CorrelationId = Guid.Parse(debit.CorrelationId) }, 200);
    }
}
