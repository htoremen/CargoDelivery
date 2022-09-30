using System.Text.Json;

namespace Cargo.Application.Cargos.Commands.DebitHistories;

public class DebitHistoryCommand : IRequest<GenericResponse<DebitHistoryResponse>>
{
    public string CourierId { get; set; }
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
        try
        {
            var debitHistory = new DebitHistory
            {
                DebitHistoryId = Guid.NewGuid().ToString(),
                UserId = request.CourierId,
                DebitId = request.DebitId,
                CargoId = request.CargoId,
                CargoItemId = request.CargoItemId,
                CommandName = request.CommandName,
                Request = request.Request,
                Response = request.Response,
                CreatedOn = DateTime.Now
            };

            _context.DebitHistories.Add(debitHistory);

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
        }
        return GenericResponse<DebitHistoryResponse>.Success(200);
    }
}
