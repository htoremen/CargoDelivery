using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cargo.Application.Consumer;
public class CreateOrderConsumer : IConsumer<ICreateOrder>
{
    private readonly IMediator _mediator;

    public CreateOrderConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task Consume(ConsumeContext<ICreateOrder> context)
    {
        var command = context.Message;
        throw new NotImplementedException();
    }
}