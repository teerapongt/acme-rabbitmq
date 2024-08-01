using Acme.PublisherApi.Features.Messages.PublishMessages;

namespace Acme.PublisherApi.Features.Messages;

public static class MessagesEndpoints
{
    public static void MapMessagesEndpoints(this WebApplication app)
    {
        var builder = app.MapGroup("messages")
            .WithTags("Messages");

        builder.MapPost("/",
                async (ISender sender,
                    IValidator<PublishMessagesRequest> validator,
                    PublishMessagesRequest request,
                    CancellationToken cancellationToken
                ) =>
                {
                    app.Logger.LogDebug("Publishing message");
                    try
                    {
                        var validationResult = await validator.ValidateAsync(request, cancellationToken);

                        if (!validationResult.IsValid)
                        {
                            app.Logger.LogWarning(
                                "Bad request to publish message {@ValidationResult}",
                                validationResult.ToDictionary()
                            );
                            return Results.ValidationProblem(validationResult.ToDictionary());
                        }

                        var response = await sender.Send(request, cancellationToken);
                        return TypedResults.Ok(response);
                    }
                    catch (Exception e)
                    {
                        app.Logger.LogError(e, "Failed to publish message");
                        return Results.Problem(e.Message);
                    }
                })
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithName("PublishMessages")
            .WithSummary("Publishes a message")
            .WithOpenApi();
    }
}
