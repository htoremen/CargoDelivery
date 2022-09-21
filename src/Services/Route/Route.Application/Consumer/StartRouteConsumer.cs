using AutoMapper;
using Route.Application.Routes.StartRoutes;
using Route.Application.Routes.StateUpdates;

namespace Route.Application.Consumer;
public class StartRouteConsumer : IConsumer<IStartRoute>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ICargoService _cargoService;

    public StartRouteConsumer(IMediator mediator, IMapper mapper, ICargoService cargoService)
    {
        _mediator = mediator;
        _mapper = mapper;
        _cargoService = cargoService;
    }

    public async Task Consume(ConsumeContext<IStartRoute> context)
    {
        var command = context.Message;

        var model = _mapper.Map<StartRouteCommand>(command);
        var cargoList = await _cargoService.GetCargoListAsync(command.CorrelationId.ToString());
        model.CargoList = cargoList;
        await _mediator.Send(model);

        var state = _mapper.Map<StateUpdateCommand>(command);
        await _mediator.Send(state);
    }
}