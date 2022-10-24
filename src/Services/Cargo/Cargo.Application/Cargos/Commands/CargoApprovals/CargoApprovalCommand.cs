using AutoMapper;
using Cargos;
using Core.Domain.MessageBrokers;
using Shipments;
using static Cargos.ICargoRejected;

namespace Cargo.Application.Cargos.CargoApprovals;

public class CargoApprovalCommand : IRequest<GenericResponse<CargoApprovalResponse>>
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public bool IsApproved { get; set; }
}

public class CargoApprovalCommandHandler : IRequestHandler<CargoApprovalCommand, GenericResponse<CargoApprovalResponse>>
{
    private readonly IMessageSender<IShipmentReceived> _shipmentReceived;
    private readonly IMessageSender<ICargoRejected> _cargoRejected;
    private readonly IMapper _mapper;

    public CargoApprovalCommandHandler(IMessageSender<IShipmentReceived> shipmentReceived, IMessageSender<ICargoRejected> cargoRejected, IMapper mapper)
    {
        _shipmentReceived = shipmentReceived;
        _cargoRejected = cargoRejected;
        _mapper = mapper;
    }

    public async Task<GenericResponse<CargoApprovalResponse>> Handle(CargoApprovalCommand request, CancellationToken cancellationToken)
    {
        if (request.IsApproved)
        {
            await _shipmentReceived.SendAsync(new ShipmentReceived
            {
                CorrelationId = request.CorrelationId,
                CurrentState = request.CurrentState,
            }, null, cancellationToken);
        }
        else
        {
            await _cargoRejected.SendAsync(new CargoRejected
            {
                CorrelationId = request.CorrelationId,
                CurrentState = request.CurrentState,
            }, null, cancellationToken);
        }
        return GenericResponse<CargoApprovalResponse>.Success(new CargoApprovalResponse { }, 200);
    }
}