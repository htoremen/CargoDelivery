﻿using Microsoft.AspNetCore.SignalR.Client;

namespace Core.Infrastructure.Notification.Web.SignalR;

public class SignalRNotification<T> : IWebNotification<T>
{
    private readonly HubConnection _connection;
    private readonly string _endpoint;
    private readonly string _eventName;

    public SignalRNotification(string endpoint, string hubName, string eventName)
    {
        _endpoint = endpoint + "/" + hubName;
        _eventName = eventName;

        _connection = new HubConnectionBuilder()
                    .WithUrl(_endpoint)
                    .WithAutomaticReconnect()
                    .Build();
    }

    public async Task SendAsync(T message, CancellationToken cancellationToken = default)
    {
        if (_connection.State != HubConnectionState.Connected)
        {
            _connection.StartAsync(cancellationToken).GetAwaiter().GetResult();
        }

        await _connection.InvokeAsync(_eventName, message, cancellationToken);
    }
}