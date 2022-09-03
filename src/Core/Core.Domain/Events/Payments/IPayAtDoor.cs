﻿namespace Payments;

public interface IPayAtDoor
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

public class PayAtDoor : IPayAtDoor
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}