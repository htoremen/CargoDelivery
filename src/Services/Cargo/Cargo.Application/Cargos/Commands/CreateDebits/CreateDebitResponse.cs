﻿namespace Cargo.Application.Cargos.CreateDebits;

public class CreateDebitResponse
{
    public Guid DebitId { get; set; }
    public Guid CorrelationId { get; set; }
}