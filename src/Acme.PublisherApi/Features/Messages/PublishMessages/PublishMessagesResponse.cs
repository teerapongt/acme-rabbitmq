namespace Acme.PublisherApi.Features.Messages.PublishMessages;

public class PublishMessagesResponse
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string Content { get; set; }
}
