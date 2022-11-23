using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using Hola.Core.Model;
using Microsoft.Extensions.Logging;

namespace Hola.Core.Helper
{
    public class RabbitMQHelper
    {
        private static readonly ILogger<RabbitMQHelper> _logger;
        public static void SendToRabbitMQ(string msg, string queueName, string hostName, int port)
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = hostName;
            factory.Port = port;

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                string message = msg;
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                    routingKey: queueName,
                    basicProperties: null,
                    body: body);
            }
        }
        public static string ReceiveFromRabbitMQ(string? queueName, string hostName, int port)
        {
            try
            {
                var factory = new ConnectionFactory();
                factory.HostName = hostName;
                factory.Port = port;
                var rabbitMqConnection = factory.CreateConnection();
                var rabbitMqChannel = rabbitMqConnection.CreateModel();

                rabbitMqChannel.QueueDeclare(queue: queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                rabbitMqChannel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                int messageCount = Convert.ToInt16(rabbitMqChannel.MessageCount(queueName));
                Console.WriteLine(" Listening to the queue. This channels has {0} messages on the queue", messageCount);

                var consumer = new EventingBasicConsumer(rabbitMqChannel);
                var message = string.Empty;
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    message = Encoding.UTF8.GetString(body);
                    RabbitMQModel.Message = message;
                    rabbitMqChannel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    Thread.Sleep(1000);
                };
                rabbitMqChannel.BasicConsume(queue: queueName,
                    autoAck: false,
                    consumer: consumer);
                return message;
            }
            catch (Exception ex)
            {
                _logger.LogError("Receive From RabbitMQ: " + ex.Message);
                return string.Empty;
            }
        }
    }

       
    
}