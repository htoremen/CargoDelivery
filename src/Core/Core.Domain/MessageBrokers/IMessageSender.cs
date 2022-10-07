using System.Threading;
using System.Threading.Tasks;

namespace Core.Domain.MessageBrokers
{
    public interface IMessageSender<T>
    {
        Task SendAsync(T message, MetaData metaData = null, CancellationToken cancellationToken = default);
        Task Publish(T message, MetaData metaData = null, CancellationToken cancellationToken = default);
    }
}
