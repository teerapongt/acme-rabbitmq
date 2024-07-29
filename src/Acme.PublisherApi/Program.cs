using Acme.Contracts;
using Acme.PublisherApi.Contracts;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add MassTransit with RabbitMQ
var rabbitMqConfigurations = builder.Configuration.GetRequiredSection("RabbitMQ");
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        var host = rabbitMqConfigurations.GetValue<string>("Host");
        var virtualHost = rabbitMqConfigurations.GetValue<string>("VirtualHost");
        var username = rabbitMqConfigurations.GetValue<string>("Username") ?? string.Empty;
        var password = rabbitMqConfigurations.GetValue<string>("Password") ?? string.Empty;
        cfg.Host(host, virtualHost, h =>
        {
            h.Username(username);
            h.Password(password);
        });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/messages",
        async (PublishMessagesRequest request, IPublishEndpoint publishEndpoint, CancellationToken cancellationToken) =>
        {
            var message = new Message { Id = Guid.NewGuid(), Timestamp = DateTime.UtcNow, Content = request.Content };
            await publishEndpoint.Publish(message, cancellationToken);

            return Results.Ok(message);
        })
    .WithName("PublishMessages")
    .WithOpenApi();

app.Run();
