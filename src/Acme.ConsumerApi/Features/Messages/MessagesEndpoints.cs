using Acme.ConsumerApi.Features.Messages.GetRecentMessages;

namespace Acme.ConsumerApi.Features.Messages;

public static class MessagesEndpoints
{
    public static void MapMessagesEndpoints(this WebApplication app)
    {
        var builder = app.MapGroup("messages")
            .WithTags("Messages");

        builder.MapGet("/{Count:int}", async (
                ISender sender,
                IValidator<GetRecentMessagesRequest> validator,
                [AsParameters] GetRecentMessagesRequest request,
                CancellationToken cancellationToken
            ) =>
            {
                app.Logger.LogDebug("Getting recent messages");

                try
                {
                    var validationResult = await validator.ValidateAsync(request, cancellationToken);

                    if (!validationResult.IsValid)
                    {
                        app.Logger.LogWarning(
                            "Bad request to get recent messages {@ValidationResult}",
                            validationResult.ToDictionary()
                        );
                        return Results.ValidationProblem(validationResult.ToDictionary());
                    }

                    var response = await sender.Send(request, cancellationToken);
                    return TypedResults.Ok(response);
                }
                catch (Exception e)
                {
                    app.Logger.LogError(e, "Failed to get recent messages");
                    return Results.Problem(e.Message);
                }
            })
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithName("GetRecentMessages")
            .WithSummary("Gets recent messages")
            .WithOpenApi();
    }
}
