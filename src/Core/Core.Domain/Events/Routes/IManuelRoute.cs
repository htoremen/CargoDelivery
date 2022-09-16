﻿namespace Routes;

public interface IManuelRoute //: IEvent
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}

public class ManuelRoute : IManuelRoute
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}