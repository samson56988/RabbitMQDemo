using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


ConnectionFactory factory = new();
factory.Uri = new Uri(uriString: "amqp://guest:guest@localhost:5672");
factory.ClientProvidedName = "Rabbit Reciever1 App";

IConnection cnn = factory.CreateConnection();
IModel channel = cnn.CreateModel();

string exchangName = "DemoExchange";
string routingKey = "demo-routing-key";
string queueName = "DemoQueue";


channel.ExchangeDeclare(exchangName, ExchangeType.Direct);
channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
channel.QueueBind(queueName, exchangName, routingKey, arguments: null);
channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (sender, args) =>
{
    var body = args.Body.ToArray();

    string message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Message Recieved: {message}");
    channel.BasicAck(args.DeliveryTag, false);
};

string consumerTag = channel.BasicConsume(queueName, autoAck: false, consumer);


Console.ReadLine();

channel.BasicCancel(consumerTag);

channel.Close();
cnn.Close();


