﻿using Deliveries;

namespace Saga.Application.Deliveries;
public class CreateDeliveryCommand : ICreateDelivery
{
    public CreateDeliveryCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public Guid CargoId { get; set; }

    public Guid CorrelationId { get; set; }
}