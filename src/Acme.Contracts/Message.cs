namespace Acme.Contracts;

public record Message
{
    public string Id { get; init; }
    public DateTime Timestamp { get; init; }
    public string Content { get; init; }
}
