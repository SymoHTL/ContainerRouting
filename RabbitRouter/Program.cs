// das sind alle cities, jede city ist nur zur nächsten city verbunden

var builder = WebApplication.CreateBuilder(args);


var cityGraph = new[] {
    Cities.Osaka,
    Cities.Kyoto,
    Cities.Tokyo,
    Cities.Busan,
    Cities.Shanghai,
    Cities.Hongkong,
    Cities.Peking,
    Cities.Moskau,
    Cities.StPetersburg,
    Cities.Hamburg,
    Cities.Rotterdam,
    Cities.Wien,
    Cities.Mailand,
    Cities.Rom,
    Cities.Turin,
    Cities.Paris,
    Cities.Barcelona,
    Cities.Lissabon,
    Cities.London
};

var nodes = new List<Node>();

for (var i = cityGraph.Length - 1; i >= 0; i--) {
    var current = cityGraph[i];
    var next = cityGraph[(i + 1) % cityGraph.Length];

    nodes.AddRange(cityGraph.Select(end => new Node(current, next, end)));
}

Configuration.Host = builder.Configuration["RabbitMQ:Host"];
Configuration.Username = builder.Configuration["RabbitMQ:Username"];
Configuration.Password = builder.Configuration["RabbitMQ:Password"];
Configuration.ExchangeName = builder.Configuration["RabbitMQ:ExchangeName"];


var factory = new ConnectionFactory {
    HostName = Configuration.Host,
    UserName = Configuration.Username,
    Password = Configuration.Password
};
var connection = factory.CreateConnection();
var channel = connection.CreateModel();


RabbitGenerator.CreateExchange(channel);
RabbitGenerator.CreateQueues(channel, nodes, (queueName, args) => {
    var container = JsonSerializer.Deserialize<Container>(args.Body.ToArray());
    if (container is null) {
        Console.WriteLine("Container is null");
        return;
    }

    Console.WriteLine($"Received container {container} at {queueName}");

    container.Node.CurrentCity = queueName;

    if (container.Node.CurrentCity == container.Node.EndCity) {
        Console.WriteLine($"Container {container.Id} reached destination {container.Node.CurrentCity}");
        return;
    }

    RabbitGenerator.PublishContainerMove(channel, container);
});


Thread.Sleep(Timeout.Infinite);