namespace Acme.Contracts;

public record Message
{
    public Guid Id { get; init; }
    public DateTime Timestamp { get; init; }
    public string Content { get; init; }
}
