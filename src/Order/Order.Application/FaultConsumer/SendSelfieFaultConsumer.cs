using MassTransit;

namespace Order.Application.FaultConsumer;

public class SendSelfieFaultConsumer : IConsumer<Fault<ISendSelfie>>
{
    private readonly IMediator _mediator;

    public SendSelfieFaultConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<Fault<ISendSelfie>> context)
    {
        var command = context.Message;
        int? maxAttempts = context.Headers.Get("MT-Redelivery-Count", default(int?));

        if (maxAttempts > 3)
        {
            throw new Exception("Something's happened during processing...");
        }
    }
}