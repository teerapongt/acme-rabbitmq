namespace Acme.PublisherApi.Features.Messages.PublishMessages;

public class PublishMessagesRequestValidator : AbstractValidator<PublishMessagesRequest>
{
    public PublishMessagesRequestValidator()
    {
        RuleFor(x=>x.Content).NotEmpty();
    }
}
