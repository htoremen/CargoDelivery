using Core.Domain.Events.Fault;

namespace Saga.Application.Cargos.Fault;

public class CreateSelfieFaultCommand : ICreateSelfieFault
{
    public CreateSelfieFaultCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    public Guid CargoId { get; set; }

    public Guid CorrelationId { get; set; }
}

