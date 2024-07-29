using Acme.ConsumerApi.Consumers;
using Acme.Contracts;
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
    x.AddConsumer<MessageConsumer>();

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

app.MapGet("/messages/{count:int}", (int count) =>
    {
        var recentMessages = MessageConsumer.GetRecentMessages(count);

        return Results.Ok(recentMessages);
    })
    .Produces<List<Message>>()
    .WithName("GetRecentMessages")
    .WithOpenApi();

app.Run();
