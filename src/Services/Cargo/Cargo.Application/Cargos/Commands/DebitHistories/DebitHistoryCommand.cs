namespace Cargo.Application.Cargos.Commands.DebitHistories;

public class DebitHistoryCommand : IRequest<GenericResponse<DebitHistoryResponse>>
{
    public string UserId { get; set; }
    public string DebitId { get; set; }
    public string CargoId { get; set; }
    public string CargoItemId { get; set; }
    public string CommandName { get; set; }
    public string Request { get; set; }
    public string Response { get; set; }
}

public class DebitHistoryCommandHandler : IRequestHandler<DebitHistoryCommand, GenericResponse<DebitHistoryResponse>>
{
    private readonly IApplicationDbContext _context;

    public DebitHistoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GenericResponse<DebitHistoryResponse>> Handle(DebitHistoryCommand request, CancellationToken cancellationToken)
    {
        _context.DebitHistories.Add(new DebitHistory
        {
            UserId = request.UserId,
            DebitId = request.DebitId,
            CargoId = request.CargoId,
            CargoItemId = request.CargoItemId,
            CommandName = request.CommandName,
            Request = request.Request,
            Response = request.Response,
        });

        await _context.SaveChangesAsync(cancellationToken);
        return GenericResponse<DebitHistoryResponse>.Success(200);
    }
}
