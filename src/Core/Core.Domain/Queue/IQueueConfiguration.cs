namespace Core.Domain
{
    public interface IQueueConfiguration
    {
        public Dictionary<QueueState, string> Names { get; set; }
    }
}
