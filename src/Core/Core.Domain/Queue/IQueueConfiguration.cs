namespace Core.Domain
{
    public interface IQueueConfiguration
    {
        public Dictionary<QueueName, string> Names { get; set; }
    }
}
