namespace Core.Infrastructure.Telemetry.Jaeger;

public class JaegerOptions
{
    public RedisOptions Redis { get; set; }
    public string AgentHost { get; set; }
    public string AgentPort { get; set; }
}
