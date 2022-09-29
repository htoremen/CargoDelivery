using Cargo.Application.Cargos.Commands.CreateCargos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cargo.Application.Consumer;

public class CreateCargoConsumer : IConsumer<ICreateCargo>
{
    private readonly IMediator _mediator;

    public CreateCargoConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task Consume(ConsumeContext<ICreateCargo> context)
    {
        var command = context.Message;
        await _mediator.Send(new CreateCargoCommand
        {
            CargoId = command.CargoId,
            Address = command.Address,
            CorrelationId = command.CorrelationId,
            DebitId = command.DebitId,
            CargoItems = command.CargoItems
        });
    }
}

