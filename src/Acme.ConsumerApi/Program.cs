using Acme.ConsumerApi.Consumers;
using Acme.ConsumerApi.Options;
using Acme.Contracts;
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
    x.AddConsumer<MessageConsumer>();

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

app.MapGet("/messages/{count:int}", (int count) =>
    {
        var recentMessages = MessageConsumer.GetRecentMessages(count);

        return Results.Ok(recentMessages);
    })
    .Produces<List<Message>>()
    .WithName("GetRecentMessages")
    .WithOpenApi();

app.Run();
