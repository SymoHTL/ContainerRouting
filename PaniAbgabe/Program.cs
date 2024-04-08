var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var factory = new ConnectionFactory {
    HostName = "localhost",
    UserName = "guest",
    Password = "guest"
};
var connection = factory.CreateConnection();
var channel = connection.CreateModel();

app.MapPut("/api/containers", ([FromBody]ContainerDto container) => {
        channel.BasicPublish("container", $"container.{container.Node.CurrentCity}.{container.Node.EndCity}",
            channel.CreateBasicProperties(), JsonSerializer.SerializeToUtf8Bytes(container));
    })
    .WithOpenApi();

var cityGraph = new[] { "Osaka", "Kyoto", "Tokyo", "Busan", "Shanghai", "Hongkong", "Peking", "Moskau", "St.Petersburg", "Hamburg", "Rotterdam", "Wien", "Mailand", "Rom", "Turin", "Paris", "Barcelona", "Lissabon", "London" };


var nodes = new List<QueueDef>();

for (var i = cityGraph.Length - 1; i >= 0; i--) {
    var current = cityGraph[i];
    var next = cityGraph[(i + 1) % cityGraph.Length];

    nodes.AddRange(cityGraph.Select(end => new QueueDef(current, next, end)));
}


channel.ExchangeDeclare("container", ExchangeType.Topic, true);

foreach (var node in nodes) {
    channel.QueueDeclare(node.NextCity, true, false, false, null);
    channel.QueueBind(node.NextCity, "container", $"container.{node.CurrentCity}.{node.EndCity}");
            
    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (_, args) => {
        var container = JsonSerializer.Deserialize<ContainerDto>(args.Body.ToArray());
        if (container is null) {
            Console.WriteLine("Container is null");
            return;
        }

        Console.WriteLine($"Received container {container} at {node.NextCity}");

        container.Node.CurrentCity = node.NextCity;

        if (container.Node.CurrentCity == container.Node.EndCity) {
            Console.WriteLine($"Container {container.Id} reached destination {container.Node.CurrentCity}");
            return;
        }

        channel.BasicPublish("container", $"container.{container.Node.CurrentCity}.{container.Node.EndCity}",
            channel.CreateBasicProperties(), JsonSerializer.SerializeToUtf8Bytes(container));
    };
    channel.BasicConsume(node.NextCity, true, consumer);
}

channel.QueueDeclare("logs", true, false, false, null);


app.Run();


internal sealed class QueueDef(string currentCity, string nextCity, string endCity) { public string CurrentCity { get; set; } = currentCity; public string NextCity { get; set; } = nextCity; public string EndCity { get; set; } = endCity; }
internal sealed class ContainerDto { public int Id { get; set; } public NodeDto Node { get; set; } = null!; }
internal sealed class NodeDto { [Required] public string CurrentCity { get; set; } = null!; [Required] public string EndCity { get; set; } = null!; }