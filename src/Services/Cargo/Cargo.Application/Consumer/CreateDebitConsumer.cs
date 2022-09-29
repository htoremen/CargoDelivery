using AutoMapper;
using Cargo.Application.Cargos.CreateDebits;

namespace Cargo.Application.Consumer;
public class CreateDebitConsumer : IConsumer<ICreateDebit>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CreateDebitConsumer(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<ICreateDebit> context)
    {
        var command = context.Message;
        var model = _mapper.Map<CreateDebitCommand>(command);
        await _mediator.Send(model);
    }
}
