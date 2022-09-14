using Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Cargo.Application.Cargos.CreateCargos;

public class CreateCargoCommand : IRequest<GenericResponse<CreateCargoResponse>>
{
    public Guid CorrelationId { get; set; }
    public Guid DebitId { get; set; }
    public Guid CourierId { get; set; }
    public string CurrentState { get; set; }
    public List<CargoDetay> Cargos { get; set; }
}

public class CreateCargoCommandHandler : IRequestHandler<CreateCargoCommand, GenericResponse<CreateCargoResponse>>
{
    private IApplicationDbContext _context;
    private IMongoRepository<DebitBson> _debitRepository;

    public CreateCargoCommandHandler(IApplicationDbContext context, IMongoRepository<DebitBson> debitRepository)
    {
        _context = context;
        _debitRepository = debitRepository;
    }

    public async Task<GenericResponse<CreateCargoResponse>> Handle(CreateCargoCommand request, CancellationToken cancellationToken)
    {
        var databaseType = DatabaseType.SQLServer;
        if (DatabaseType.SQLServer == databaseType)
        {
            var debit = await CreateCargoSQLServer(request, cancellationToken); 
            return GenericResponse<CreateCargoResponse>.Success(new CreateCargoResponse { DebitId = Guid.Parse(debit.DebitId), CorrelationId = Guid.Parse(debit.CorrelationId) }, 200);
        }
        else if (DatabaseType.Mongo == databaseType)
        {
            var debit = await CreateCargoMongo(request, cancellationToken);
            return GenericResponse<CreateCargoResponse>.Success(new CreateCargoResponse { DebitId = Guid.Parse(debit.DebitId), CorrelationId = Guid.Parse(debit.CorrelationId) }, 200);

        }
        return GenericResponse<CreateCargoResponse>.NotFoundException("", 404);
    }

    private async Task<Debit> CreateCargoSQLServer(CreateCargoCommand request, CancellationToken cancellationToken)
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
                CurrentState = request.CurrentState,
                Cargos = cargos
            }).Entity;
            await _context.SaveChangesAsync(cancellationToken);
        }
        return debit;
    }

    private async Task<DebitBson> CreateCargoMongo(CreateCargoCommand request, CancellationToken cancellationToken)
    {
        var cargos = new List<CargoBson>();
        foreach (var cargo in request.Cargos)
        {
            var cargoItems = new List<CargoItemBson>();
            foreach (var cargoItem in cargo.CargoItems)
            {
                cargoItems.Add(new CargoItemBson
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
            cargos.Add(new CargoBson
            {
                CargoId = Guid.NewGuid().ToString(),
                Address = cargo.Address,
                CargoItems = cargoItems
            });
        }

        var debit = new DebitBson
        {
            DebitId = request.DebitId.ToString(),
            CorrelationId = request.CorrelationId.ToString(),
            CourierId = request.CourierId.ToString(),
            DistributionDate = DateTime.Now.Date,
            StartingDate = DateTime.Now,
            IsApproval = false,
            IsCompleted = false,
            Cargos = cargos
        };

        await _debitRepository.InsertOneAsync(debit);
        return debit;
    }
 
}
