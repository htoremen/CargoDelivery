namespace Core.Domain
{
    public class QueueConfiguration : IQueueConfiguration
    {
        public Dictionary<QueueState, string> Names { get; set; }
    }
}
