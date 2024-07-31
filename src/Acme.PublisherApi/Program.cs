using System.Reflection;
using Acme.PublisherApi.Features.Messages;
using Acme.PublisherApi.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSerilog(cfg => cfg.ReadFrom.Configuration(builder.Configuration));

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

// Add FluentValidation
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// Add MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    // cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
});

var app = builder.Build();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapMessagesEndpoints();

app.Run();
