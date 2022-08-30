using MassTransit;

namespace Order.Application.FaultConsumer;

public class CreateSelfieFaultConsumer : IConsumer<Fault<ICreateSelfie>>
{
    private readonly IMediator _mediator;

    public CreateSelfieFaultConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<Fault<ICreateSelfie>> context)
    {
        var command = context.Message;
        int? maxAttempts = context.Headers.Get("MT-Redelivery-Count", default(int?));

        if (maxAttempts > 3)
        {
            throw new Exception("Something's happened during processing...");
        }
    }
}