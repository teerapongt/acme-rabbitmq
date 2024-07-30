using Acme.Contracts;
using Acme.PublisherApi.Contracts;
using Acme.PublisherApi.Options;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add MassTransit with RabbitMQ
var rabbitMqOptions = builder.Configuration.GetRequiredSection(RabbitMqOptions.RabbitMq).Get<RabbitMqOptions>();
builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitMqOptions!.Host, rabbitMqOptions.VirtualHost, h =>
        {
            h.Username(rabbitMqOptions.Username);
            h.Password(rabbitMqOptions.Password);
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
