using System.Text.Json;
using BetterEntryPoint;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var exchangeName = builder.Configuration["RabbitMQ:ExchangeName"];

var factory = new ConnectionFactory {
    HostName = builder.Configuration["RabbitMQ:Host"],
    UserName = builder.Configuration["RabbitMQ:Username"],
    Password = builder.Configuration["RabbitMQ:Password"]
};
var connection = factory.CreateConnection();
var channel = connection.CreateModel();


app.MapPut("/api/containers", ([FromBody] ContainerDto container) => {
        channel.BasicPublish(exchangeName, $"container.{container.Node.CurrentCity}.{container.Node.EndCity}",
            channel.CreateBasicProperties(), JsonSerializer.SerializeToUtf8Bytes(container));
    })
    .WithOpenApi();

app.Run();