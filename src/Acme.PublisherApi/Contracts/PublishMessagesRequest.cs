namespace Acme.PublisherApi.Contracts;

public record PublishMessagesRequest
{
    public required string Content { get; init; }
}
