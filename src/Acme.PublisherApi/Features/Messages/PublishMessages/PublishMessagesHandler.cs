using Acme.Contracts;

namespace Acme.PublisherApi.Features.Messages.PublishMessages;

public class PublishMessagesHandler(IPublishEndpoint publishEndpoint)
    : IRequestHandler<PublishMessagesRequest, PublishMessagesResponse>
{
    public async Task<PublishMessagesResponse> Handle(
        PublishMessagesRequest request,
        CancellationToken cancellationToken
    )
    {
        var message = new Message { Id = Guid.NewGuid(), Timestamp = DateTime.UtcNow, Content = request.Content };
        await publishEndpoint.Publish(message, cancellationToken);

        var response = new PublishMessagesResponse
        {
            Id = message.Id, Timestamp = message.Timestamp, Content = message.Content
        };

        return response;
    }
}
