﻿namespace Saga.Application.Deliveries;

public class ShiftCompletionCommand : IShiftCompletion
{
    public ShiftCompletionCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public Guid CargoId { get; set; }

    public Guid CorrelationId { get; set; }
}