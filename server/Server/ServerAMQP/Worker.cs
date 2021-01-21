using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Server.Data;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ServerAMQP
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IActionsRepository _actionsRepository;


        public Worker(ILogger<Worker> logger, IActionsRepository actionsRepository)
        {
            _logger = logger;
            _actionsRepository = actionsRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var bindingKey = "Scooter.Scooter1.Summary";
            var factory = new ConnectionFactory()
            {
                Uri = new Uri("amqps://lhjvzajb:Tsu7NLVlJsIjtesiP-28UdzMMuEXif9l@bonobo.rmq.cloudamqp.com/lhjvzajb")
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "topic_logs", type: "topic");
                var queueName = channel.QueueDeclare().QueueName;

                
                
                    channel.QueueBind(queue: queueName,
                                      exchange: "topic_logs",
                                      routingKey: bindingKey);
                

                Console.WriteLine(" [*] Waiting for messages. To exit press CTRL+C");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var routingKey = ea.RoutingKey;
                    Console.WriteLine(" [x] Received '{0}':'{1}'",
                                      routingKey,
                                      message);
                    var detection = JsonSerializer.Deserialize<DataModel>(message);

                    _actionsRepository.TableServiceAsync(detection);
                };
                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }

        }
    }
}
