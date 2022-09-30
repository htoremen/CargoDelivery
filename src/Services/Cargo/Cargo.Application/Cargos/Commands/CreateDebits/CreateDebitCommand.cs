using Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Cargo.Application.Cargos.CreateDebits;

public class CreateDebitCommand : IRequest<GenericResponse<CreateDebitResponse>>
{
    public Guid CorrelationId { get; set; }
    public Guid DebitId { get; set; }
    public Guid CourierId { get; set; }
    public string CurrentState { get; set; }
    public List<CreateDebitCargo> Cargos { get; set; }
}

public class CreateDebitCommandHandler : IRequestHandler<CreateDebitCommand, GenericResponse<CreateDebitResponse>>
{
    private IApplicationDbContext _context;
    private IMongoRepository<DebitBson> _debitRepository;

    public CreateDebitCommandHandler(IApplicationDbContext context, IMongoRepository<DebitBson> debitRepository)
    {
        _context = context;
        _debitRepository = debitRepository;
    }

    public async Task<GenericResponse<CreateDebitResponse>> Handle(CreateDebitCommand request, CancellationToken cancellationToken)
    {
        var databaseType = DatabaseType.SQLServer;
        if (DatabaseType.SQLServer == databaseType)
        {
            var debitId = await CreateCargoSQLServer(request, cancellationToken); 
            return GenericResponse<CreateDebitResponse>.Success(new CreateDebitResponse { DebitId = debitId, CorrelationId = request.CorrelationId }, 200);
        }
        else if (DatabaseType.Mongo == databaseType)
        {
            var debit = await CreateCargoMongo(request, cancellationToken);
            return GenericResponse<CreateDebitResponse>.Success(new CreateDebitResponse { DebitId = debit.DebitId.ToString(), CorrelationId = Guid.Parse(debit.CorrelationId) }, 200);

        }
        return GenericResponse<CreateDebitResponse>.NotFoundException("", 404);
    }

    private async Task<string> CreateCargoSQLServer(CreateDebitCommand request, CancellationToken cancellationToken)
    {
        var debit = await _context.Debits.FirstOrDefaultAsync(x => x.CorrelationId == request.CorrelationId.ToString() && x.CourierId == request.CourierId.ToString());
        if (debit == null)
        {
            var cargos = new List<Domain.Entities.Cargo>();
            foreach (var cargo in request.Cargos)
            {
                var cargoItems = new List<CargoItem>();
                foreach (var cargoItem in cargo.CargoItems)
                {
                    cargoItems.Add(new CargoItem
                    {
                        CargoItemId = cargoItem.CargoItemId.ToString(),
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
                    CargoId = cargo.CargoId.ToString(),
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
        return debit.DebitId;
    }

    private async Task<DebitBson> CreateCargoMongo(CreateDebitCommand request, CancellationToken cancellationToken)
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


    public async Task<GenericResponse<CreateDebitResponse>> _Handle(CreateDebitCommand request, CancellationToken cancellationToken)
    {
        var debit = await _context.Debits.FirstOrDefaultAsync(x => x.DistributionDate != DateTime.Now && x.CourierId == request.CourierId.ToString());
        if (debit == null)
        {
            debit = _context.Debits.Add(new Debit
            {
                DebitId = request.DebitId.ToString(),
                CorrelationId = request.CorrelationId.ToString(),
                CourierId = request.CourierId.ToString(),
                DistributionDate = DateTime.Now.Date,
                StartingDate = DateTime.Now,
                IsApproval = false,
                IsCompleted = false,
                CurrentState = request.CurrentState
            }).Entity;
            await _context.SaveChangesAsync(cancellationToken);
            return GenericResponse<CreateDebitResponse>.Success(new CreateDebitResponse { CorrelationId = request.CorrelationId, DebitId = debit.DebitId }, 200);
        }
        return GenericResponse<CreateDebitResponse>.NotFoundException("", 404);
    }


}
