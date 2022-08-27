using Core.Domain.Enums;

namespace Cargo.Application.Cargos.CargoSendApproveds;

public class CargoSendApprovedCommand : IRequest<GenericResponse<CargoSendApprovedResponse>>
{
    public Guid Id { get; set; }
}

public class CargoSendApprovedCommandHandler : IRequestHandler<CargoSendApprovedCommand, GenericResponse<CargoSendApprovedResponse>>
{
    private readonly IEventBusService<IEventBus> _eventBusService;
    private readonly IQueueConfiguration _queueConfiguration;

    public CargoSendApprovedCommandHandler(IEventBusService<IEventBus> eventBusService, IQueueConfiguration queueConfiguration)
    {
        _eventBusService = eventBusService;
        _queueConfiguration = queueConfiguration;
    }

    public async Task<GenericResponse<CargoSendApprovedResponse>> Handle(CargoSendApprovedCommand request, CancellationToken cancellationToken)
    {
        var rnd = new Random();
        if (rnd.Next(1, 1000) % 2 == 0)
        {
            var orderApproved = new CargoApproved()
            {
                Id = Guid.NewGuid(),
            };

            await _eventBusService.SendCommandAsync(orderApproved, _queueConfiguration.Names[QueueName.CargoApproved], cancellationToken);
        }
        else
        {
            var orderRejected = new CargoRejected()
            {
                Id = Guid.NewGuid(),
            };
            await _eventBusService.SendCommandAsync(orderRejected, _queueConfiguration.Names[QueueName.CargoRejected], cancellationToken);
        }

        return GenericResponse<CargoSendApprovedResponse>.Success(new CargoSendApprovedResponse { }, 200);
    }
}
