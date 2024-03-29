﻿namespace Events;

public interface IStartDistribution
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
    public string CurrentState { get; set; }
    public NotificationType NotificationType { get; set; }
}

public class StartDistribution : IStartDistribution
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
    public string CurrentState { get; set; }
    public NotificationType NotificationType { get; set; }
}