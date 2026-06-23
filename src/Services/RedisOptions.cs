namespace simple_container.Services;

public class RedisOptions
{
    public string Configuration { get; set; } = "localhost:6379";
    public string InstanceName { get; set; } = "demo";
}
