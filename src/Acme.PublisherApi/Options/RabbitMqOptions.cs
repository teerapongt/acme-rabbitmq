namespace Acme.PublisherApi.Options;

public class RabbitMqOptions
{
    public const string RabbitMq = "RabbitMQ";

    public string Host { get; set; } = string.Empty;
    public string VirtualHost { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
