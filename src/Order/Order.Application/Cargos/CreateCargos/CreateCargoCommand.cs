namespace Cargo.Application.Cargos.CreateCargos;

public class CreateCargoCommand : IRequest<CreateCargoResponse>
{
    public Guid DebitId { get; set; }
    public Guid CourierId { get; set; }
    public List<CargoDetay> Cargos { get; set; }
}

public class CreateCargoCommandHandler : IRequestHandler<CreateCargoCommand, CreateCargoResponse>
{
    private readonly IMessageSender<ICreateCargo> _createCargo;

    public CreateCargoCommandHandler(IMessageSender<ICreateCargo> createCargo)
    {
        _createCargo = createCargo;
    }

    public async Task<CreateCargoResponse> Handle(CreateCargoCommand request, CancellationToken cancellationToken)
    {
        await _createCargo.SendAsync(new CreateCargo
        {
            DebitId = request.DebitId,
            CourierId = request.CourierId,
            Cargos = request.Cargos,
        }, null, cancellationToken);
        return new CreateCargoResponse { DebitId = request.DebitId, CourierId = request.CourierId };
    }
}