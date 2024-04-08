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


app.MapPut("/api/containers", ([FromBody]ContainerDto container) => {
        Configuration.Host = "localhost";
        Configuration.Username = "guest";
        Configuration.Password = "guest";
        Configuration.ExchangeName = "container";


        var factory = new ConnectionFactory {
            HostName = Configuration.Host,
            UserName = Configuration.Username,
            Password = Configuration.Password
        };
        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();
        
        
        channel.BasicPublish(Configuration.ExchangeName, $"container.{container.Node.CurrentCity}.{container.Node.EndCity}",
            channel.CreateBasicProperties(), JsonSerializer.SerializeToUtf8Bytes(container));
    })
    .WithOpenApi();

app.Run();
