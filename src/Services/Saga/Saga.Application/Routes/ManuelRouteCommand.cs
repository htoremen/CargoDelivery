﻿namespace Saga.Application.Routes;

public class ManuelRouteCommand : IManuelRoute
{
    public ManuelRouteCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}
