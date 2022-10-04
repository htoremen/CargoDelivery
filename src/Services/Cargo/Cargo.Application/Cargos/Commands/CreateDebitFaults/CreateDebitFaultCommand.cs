namespace Cargo.Application.Cargos.Commands.CreateDebitFaults;

public class CreateDebitFaultCommand : IRequest<GenericResponse<CreateDebitFaultResponse>>
{
    public Guid CorrelationId { get; set; }
    public Guid DebitId { get; set; }
    public Guid CourierId { get; set; }
    public string CurrentState { get; set; }
    public List<CreateDebitCargo> Cargos { get; set; }
}


public class CreateDebitFaultCommandHandler : IRequestHandler<CreateDebitFaultCommand, GenericResponse<CreateDebitFaultResponse>>
{
    private IMongoRepository<DebitBson> _debitRepository;

    public CreateDebitFaultCommandHandler(IMongoRepository<DebitBson> debitRepository)
    {
        _debitRepository = debitRepository;
    }

    public async Task<GenericResponse<CreateDebitFaultResponse>> Handle(CreateDebitFaultCommand request, CancellationToken cancellationToken)
    {
        var isDebit = _debitRepository.FilterBy(x => x.DebitId == request.DebitId.ToString()).FirstOrDefault();
        if(isDebit != null)
            return  GenericResponse<CreateDebitFaultResponse>.Success(200);

        var cargos = new List<CargoBson>();
        foreach (var cargo in request.Cargos)
        {
            var cargoItems = new List<CargoItemBson>();
            foreach (var cargoItem in cargo.CargoItems)
            {
                cargoItems.Add(new CargoItemBson
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
            cargos.Add(new CargoBson
            {
                CargoId = cargo.CargoId.ToString(),
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
        return GenericResponse<CreateDebitFaultResponse>.Success(new CreateDebitFaultResponse
        {
            CorrelationId = request.CorrelationId,
            CurrentState = request.CurrentState
        }, 200);
    }
}

