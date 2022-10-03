using Core.Application.Common.Interfaces;

namespace Cargo.Application.Cargos.CreateDebits;

public class CreateDebitCommand : IRequest<CreateDebitResponse>
{
    public Guid DebitId { get; set; }
    public Guid CourierId { get; set; }
    public List<CreateDebitCargo> Cargos { get; set; }
}

public class CreateDebitCommandHandler : IRequestHandler<CreateDebitCommand, CreateDebitResponse>
{
    private readonly IMessageSender<ICreateDebit> _createCargo;
    private readonly ICacheService _cacheService;

    public CreateDebitCommandHandler(IMessageSender<ICreateDebit> createCargo, ICacheService cacheService)
    {
        _createCargo = createCargo;
        _cacheService = cacheService;
    }

    public async Task<CreateDebitResponse> Handle(CreateDebitCommand request, CancellationToken cancellationToken)
    {
        await _cacheService.SetAsync(request.DebitId.ToString(), request);

        await _createCargo.SendAsync(new CreateDebit
        {
            DebitId = request.DebitId,
            CourierId = request.CourierId,
            Cargos = request.Cargos,
        }, null, cancellationToken);
        return new CreateDebitResponse { DebitId = request.DebitId, CourierId = request.CourierId };
    }
}