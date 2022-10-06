using AutoMapper;
using Core.Domain.MessageBrokers;

namespace Cargo.Application.Cargos.CargoApprovals;

public class CargoApprovalCommand : IRequest<GenericResponse<CargoApprovalResponse>>
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}

public class CargoApprovalCommandHandler : IRequestHandler<CargoApprovalCommand, GenericResponse<CargoApprovalResponse>>
{
    private readonly IMessageSender<IStartRoute> _startRoute;
    private readonly IMapper _mapper;

    public CargoApprovalCommandHandler(IMessageSender<IStartRoute> startRoute, IMapper mapper)
    {
        _startRoute = startRoute;
        _mapper = mapper;
    }

    public async Task<GenericResponse<CargoApprovalResponse>> Handle(CargoApprovalCommand request, CancellationToken cancellationToken)
    {
        await _startRoute.SendAsync(new StartRoute
        {
            CurrentState = request.CurrentState,
            CorrelationId = request.CorrelationId
        }, null, cancellationToken);
        return GenericResponse<CargoApprovalResponse>.Success(new CargoApprovalResponse { }, 200);
    }
}