using System.Diagnostics;

namespace Sage.Application.Telemetry;

public static class ConsumerActivitySource
{
    public static readonly ActivitySource Source = OpenTelemetryExtensions.CreateActivitySource();
}

