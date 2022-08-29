namespace Saga.Application.Cargos;

public class CreateSelfieCommand : ICreateSelfie
{
    public CreateSelfieCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    public Guid CargoId { get; set;}

    public Guid CorrelationId { get; }
}
