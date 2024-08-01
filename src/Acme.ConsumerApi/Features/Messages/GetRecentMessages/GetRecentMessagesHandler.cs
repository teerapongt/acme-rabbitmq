using Acme.ConsumerApi.Consumers;

namespace Acme.ConsumerApi.Features.Messages.GetRecentMessages;

public class GetRecentMessagesHandler : IRequestHandler<GetRecentMessagesRequest, List<GetRecentMessagesResponse>>
{
    public Task<List<GetRecentMessagesResponse>> Handle(GetRecentMessagesRequest request,
        CancellationToken cancellationToken)
    {
        var recentMessages = RecentMessageConsumer.GetRecentMessages(request.Count).ToList();

        var response = recentMessages.Select(x =>
            new GetRecentMessagesResponse { Id = x.Id, Timestamp = x.Timestamp, Content = x.Content }).ToList();

        return Task.FromResult(response);
    }
}
