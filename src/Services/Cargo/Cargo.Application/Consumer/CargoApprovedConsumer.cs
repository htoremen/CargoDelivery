﻿using Cargo.Application.Cargos.CargoApproveds;

namespace Cargo.Application.Consumer;

public class CargoApprovedConsumer : IConsumer<IOrderApproved>
{
    private readonly IMediator _mediator;

    public CargoApprovedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<IOrderApproved> context)
    {
        var command = context.Message;

        await _mediator.Send(new CargoApprovedCommand
        {
            Id = command.Id
        });
    }
}

