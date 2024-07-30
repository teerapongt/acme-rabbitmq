namespace Acme.PublisherApi.Features.Messages.PublishMessages;

public record struct PublishMessagesRequest(string Content) : IRequest<PublishMessagesResponse>;
