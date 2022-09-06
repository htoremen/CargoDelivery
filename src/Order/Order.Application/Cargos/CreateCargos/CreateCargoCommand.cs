namespace Cargo.Application.Cargos.CreateOrders;

public class CreateCargoCommand : IRequest<CreateCargoResponse>
{
    public Guid Id { get; set; }
    public Guid CargoId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid ProductId { get; set; }
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
            CustomerId = request.CustomerId,
            CargoId = request.CargoId,
            ProductId = request.ProductId
        }, null, cancellationToken);
        return new CreateCargoResponse { Id = request.Id, CargoId = request.CargoId };
    }
}