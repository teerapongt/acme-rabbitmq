using Acme.PublisherApi.Features.Messages.PublishMessages;

namespace Acme.PublisherApi.Features.Messages;

public static class MessagesEndpoints
{
    public static void MapMessagesEndpoints(this WebApplication app)
    {
        var builder = app.MapGroup("messages")
            .WithTags("Messages");

        builder.MapPost("/",
                async (ISender sender, PublishMessagesRequest request, CancellationToken cancellationToken) =>
                {
                    var response = await sender.Send(request, cancellationToken);
                    return TypedResults.Ok(response);
                })
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithName("PublishMessages")
            .WithSummary("Publishes a message")
            .WithOpenApi();
    }
}
