﻿using Enums;

namespace Order.Application.Deliveries.CreateDeliveries;

public class CreateDeliveryResponse
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
    public PaymentType PaymentType { get; set; }
}
