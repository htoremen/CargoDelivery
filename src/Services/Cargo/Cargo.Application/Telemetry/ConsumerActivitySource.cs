using System.Diagnostics;

namespace Cargo.Application.Telemetry;

public static class ConsumerActivitySource
{
    public static readonly ActivitySource Source = OpenTelemetryExtensions.CreateActivitySource();
}

