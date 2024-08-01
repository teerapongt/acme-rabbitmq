namespace Acme.ConsumerApi.Features.Messages.GetRecentMessages;

public class GetRecentMessagesResponse
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string Content { get; set; }
}
