using CamScanner.Models;
using CamScanner.Services;
using MassTransit;
using MassTransit.Serialization;
using RabbitMQ.Client;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var rabbitMqSettings = builder.Configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();

builder.Services.AddMassTransit(mt => mt.AddMassTransit(x =>
{
    x.AddConsumer<MessageConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitMqSettings.URL, 5672, "/", c =>
        {
            c.Username(rabbitMqSettings.UserName);
            c.Password(rabbitMqSettings.Password);
        });
        cfg.ClearSerialization();
        cfg.UseRawJsonSerializer(RawSerializerOptions.All);

        cfg.ReceiveEndpoint("download", e => { e.ConfigureConsumer<MessageConsumer>(context); });
        cfg.Durable = true;
    });
    x.SetKebabCaseEndpointNameFormatter();
    
}));
builder.Services.AddSignalR(e =>
{
    e.EnableDetailedErrors = true;
    e.MaximumParallelInvocationsPerClient = 100;
});
// builder.Services.AddMassTransitHostedService();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();