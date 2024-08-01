using Microsoft.AspNetCore.Mvc;

namespace Acme.ConsumerApi.Features.Messages.GetRecentMessages;

public record struct GetRecentMessagesRequest([FromRoute] int Count) : IRequest<List<GetRecentMessagesResponse>>;
