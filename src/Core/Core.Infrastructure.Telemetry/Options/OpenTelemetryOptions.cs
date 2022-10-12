namespace Core.Infrastructure.Telemetry.Options;

public class OpenTelemetryOptions
{
    public string RedisConfiguration { get; set; }
    public string ServiceName { get; set; }
    public string ServiceVersion { get; set; }
    public string AgentHost { get; set; }
    public string AgentPort { get; set; }
}
