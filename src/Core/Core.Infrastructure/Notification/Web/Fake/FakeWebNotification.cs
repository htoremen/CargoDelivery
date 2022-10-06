using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Notification.Web;

public class FakeWebNotification<T> : IWebNotification<T>
{
    public Task SendAsync(T message, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}