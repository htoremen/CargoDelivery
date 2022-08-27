namespace Cargo.Application.Cargos.CreateCargos;

public class CreateCargoCommand : IRequest<Response<CreateCargoResponse>>
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid ProductId { get; set; }
}

public class CreateCargoCommandHandler : IRequestHandler<CreateCargoCommand, Response<CreateCargoResponse>>
{
    public async Task<Response<CreateCargoResponse>> Handle(CreateCargoCommand request, CancellationToken cancellationToken)
    {
        return Response<CreateCargoResponse>.Success(new CreateCargoResponse {  }, 200);
    }
}
