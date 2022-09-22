using AutoMapper;
using Cargo.Application.Cargos.CreateCargos;

namespace Cargo.Application.Consumer;
public class CreateCargoConsumer : IConsumer<ICreateCargo>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CreateCargoConsumer(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<ICreateCargo> context)
    {
        var command = context.Message;
        var model = _mapper.Map<CreateCargoCommand>(command);
        await _mediator.Send(model);
    }
}
