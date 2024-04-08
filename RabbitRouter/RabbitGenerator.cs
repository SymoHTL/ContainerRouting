namespace RabbitRouter;

public class RabbitGenerator {
    
    public delegate void ContainerMsg<in T>(string queueName, T args);
    public static void CreateQueues(IModel model, List<Node> nodes,  ContainerMsg<BasicDeliverEventArgs> onReceived) {
        foreach (var node in nodes) {
            model.QueueDeclare(node.NextCity, true, false, false, null);
            model.QueueBind(node.NextCity, Configuration.ExchangeName, $"container.{node.CurrentCity}.{node.EndCity}");
            
            var consumer = new EventingBasicConsumer(model);
            consumer.Received += (_, args) => {
                onReceived(node.NextCity, args);
            };
            model.BasicConsume(node.NextCity, true, consumer);
        }

        model.QueueDeclare("logs", true, false, false, null);
        model.QueueBind("logs", Configuration.ExchangeName, "container.#");
    }

    public static void CreateExchange(IModel model) {
        model.ExchangeDeclare(Configuration.ExchangeName, ExchangeType.Topic, true);
    }

    public static void PublishContainerMove(IModel model, Container container) {
        model.BasicPublish(Configuration.ExchangeName, $"container.{container.Node.CurrentCity}.{container.Node.EndCity}",
            model.CreateBasicProperties(), JsonSerializer.SerializeToUtf8Bytes(container));
    }
    
}
