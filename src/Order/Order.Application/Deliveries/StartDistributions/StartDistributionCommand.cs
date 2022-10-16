namespace Order.Application.Deliveries.StartDistributions;

public class StartDistributionCommand : IRequest<Unit>
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}

public class StartDistributionCommandHandler : IRequestHandler<StartDistributionCommand>
{
    private readonly IMessageSender<IStartDistribution> _startDistribution;

    public StartDistributionCommandHandler(IMessageSender<IStartDistribution> startDistribution)
    {
        _startDistribution = startDistribution;
    }

    public async Task<Unit> Handle(StartDistributionCommand request, CancellationToken cancellationToken)
    {
        await _startDistribution.SendAsync(new StartDistribution
        {
            CargoId = request.CargoId,
            CorrelationId = request.CorrelationId
        });
        return Unit.Value;
    }
}