using Core.Infrastructure.Telemetry.Jaeger;

namespace Core.Infrastructure.Telemetry;

public class TelemetryOptions
{
    public string Provider { get; set; }
    public JaegerOptions Jaeger { get; set; }
}
