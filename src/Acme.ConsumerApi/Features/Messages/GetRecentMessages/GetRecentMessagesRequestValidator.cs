namespace Acme.ConsumerApi.Features.Messages.GetRecentMessages;

public class GetRecentMessagesRequestValidator : AbstractValidator<GetRecentMessagesRequest>
{
    public GetRecentMessagesRequestValidator()
    {
        RuleFor(x => x.Count).GreaterThan(0);
    }
}
