using RabbitMQ.Client;
using System.Text;


ConnectionFactory factory = new();
factory.Uri = new Uri(uriString: "amqp://guest:guest@localhost:5672");
factory.ClientProvidedName = "Rabbit Sender App";

IConnection cnn = factory.CreateConnection();
IModel channel = cnn.CreateModel();

string exchangName = "DemoExchange";
string routingKey = "demo-routing-key";
string queueName = "DemoQueue";


channel.ExchangeDeclare(exchangName, ExchangeType.Direct);
channel.QueueDeclare(queueName,durable:false,exclusive:false,autoDelete:false,arguments:null);
channel.QueueBind(queueName, exchangName, routingKey, arguments: null);


byte[] messageBodyBytes = Encoding.UTF8.GetBytes(s: "Hello YouTube");
channel.BasicPublish(exchangName,routingKey,basicProperties:null,messageBodyBytes);

channel.Close();
cnn.Close();






