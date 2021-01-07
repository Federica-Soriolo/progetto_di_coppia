using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Server.Data;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace ServerMqtt
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private static MqttClient client;
        private readonly IActionsRepository _actionsRepository;

        public Worker(ILogger<Worker> logger, IActionsRepository actionsRepository)
        {
            _logger = logger;
            _actionsRepository = actionsRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string topic = "Scooter/#";

            string BrokerAddress = "127.0.0.1";

            client = new MqttClient(BrokerAddress);

            client.MqttMsgPublishReceived += PublishReceived;

            string clientId = Guid.NewGuid().ToString();

            client.Connect(clientId);

            Subscribe(topic);

            
        }

        private void PublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string ReceivedMessage = Encoding.UTF8.GetString(e.Message);
            var detection = JsonSerializer.Deserialize<DataModel>(ReceivedMessage);

            _actionsRepository.TableServiceAsync(detection);

            _logger.LogInformation($"Message: {ReceivedMessage}");
        }

        private void Subscribe(string topic)
        {
            if (topic != "")
            {

                client.Subscribe(new string[] { topic }, new byte[] { 2 });
                _logger.LogInformation($"Ok subscribed topic: {topic}");
            }
            else
            {
                _logger.LogInformation("Invalid topic.");
            }
        }
    }
}
