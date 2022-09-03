﻿namespace Deliveries;

public interface IShiftCompletion
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

public class ShiftCompletion : IShiftCompletion
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}