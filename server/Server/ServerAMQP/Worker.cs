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
        private readonly string username = "alessia.colin@stud.tecnicosuperiorekennedy.it";
        private readonly string password = "Tsu7NLVlJsIjtesiP-28UdzMMuEXif9l";
        private readonly string virtualhost = "lhjvzajb";

        public Worker(ILogger<Worker> logger, IActionsRepository actionsRepository)
        {
            _logger = logger;
            _actionsRepository = actionsRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory()
            {
                Uri = new Uri("amqps://lhjvzajb:Tsu7NLVlJsIjtesiP-28UdzMMuEXif9l@bonobo.rmq.cloudamqp.com/lhjvzajb")
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "Scooter",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    _logger.LogInformation($"Message: {message}");
                    var detection = JsonSerializer.Deserialize<DataModel>(message);

                    _actionsRepository.TableServiceAsync(detection);
                };
                channel.BasicConsume(queue: "Scooter",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
            
        }
    }
}
