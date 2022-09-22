using Delivery.Application.Cargos.Queries.GetCargoAlls;
using Delivery.Application.Cargos.Queries.GetRoutes.GetRouteQuery;
using Delivery.Application.Deliveries.StartDeliveries;

namespace Delivery.Application.Consumer;

public class StartDeliveryConsumer : IConsumer<IStartDelivery>
{
    private readonly IMediator _mediator;
    private readonly IMessageSender<INewDelivery> _newDelivery;

    public StartDeliveryConsumer(IMediator mediator, IMessageSender<INewDelivery> newDelivery)
    {
        _mediator = mediator;
        _newDelivery = newDelivery;
    }
    public async Task Consume(ConsumeContext<IStartDelivery> context)
    {
        var command = context.Message;

        var result = await _mediator.Send(new GetCargoAllQuery { CorrelationId = command.CorrelationId.ToString() });
        var routes = await _mediator.Send(new GetRouteQuery { CorrelationId = command.CorrelationId.ToString() });

        await _mediator.Send(new StartDeliveryCommand
        {
            CurrentState = command.CurrentState,
            CorrelationId = command.CorrelationId,
            Routes = routes.Data.Routes.ToList(),
            Cargos = result.Data.Cargos.ToList()
        });

        await _newDelivery.SendAsync(new NewDelivery
        {
            CorrelationId = command.CorrelationId,
            CurrentState = command.CurrentState
        }, null);
    }
}
