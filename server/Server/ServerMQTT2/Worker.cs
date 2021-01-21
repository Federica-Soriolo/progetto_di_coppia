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
using System.Timers;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace ServerMqtt
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private static MqttClient client;
        private readonly IActionsRepository _actionsRepository;
        private static System.Timers.Timer batterytimer;

        public Worker(ILogger<Worker> logger, IActionsRepository actionsRepository)
        {
            _logger = logger;
            _actionsRepository = actionsRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string topic = "Scooter/Scooter1/Summary";

            string BrokerAddress = "127.0.0.1";

            client = new MqttClient(BrokerAddress);

            client.MqttMsgPublishReceived += PublishReceived;

            string clientId = Guid.NewGuid().ToString();

            client.Connect(clientId);

            Subscribe(topic);
            Subscribe("Scooter/Scooter1/LastWillTopic");

            Publish("Scooter/Scooter1/Cmd/Scooter", "Scooter ON"); //Invia il comando di accensione monopattino
            Publish("Scooter/Scooter1/Cmd/Race", "Start race"); //Invia il comando di apertura nuova corsa
            Publish("Scooter/Scooter1/Cmd/Led", "Scooter LED ON"); //Invia il comando di accensione led di pos monopattino
            Publish("Scooter/Scooter1/Cmd/Display", "Scooter Display ON"); //Invia il comando di accensione display monopattino

            SetTimer();
        }

        private void PublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string ReceivedMessage = Encoding.UTF8.GetString(e.Message);
            if (ReceivedMessage != null)
            {
                _logger.LogInformation($"Message: {ReceivedMessage}");
                //var detection = JsonSerializer.Deserialize<DataModel>(ReceivedMessage);

                //_actionsRepository.TableServiceAsync(detection);

                
            }

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

        public static void Publish(string topic, string message)
        {
            if (topic != "")
            {
                client.Publish(topic, Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
                Console.WriteLine($"Ok published message:{message} on topic:{topic}");

            }
            else
            {
                Console.WriteLine("Invalid topic.");
            }
        }

        private static void SetTimer()
        {
            // Create a timer with a two second interval.
            batterytimer = new System.Timers.Timer(60000);
            // Hook up the Elapsed event for the timer. 
            batterytimer.Elapsed += OnTimedEvent;
            batterytimer.AutoReset = true;
            batterytimer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Publish("Scooter/Scooter1/Cmd/Scooter", "Scooter OFF"); //Invia il comando di accensione monopattino
            Publish("Scooter/Scooter1/Cmd/Race", "Stop race"); //Invia il comando di apertura nuova corsa
            Publish("Scooter/Scooter1/Cmd/Led", "Scooter LED OFF"); //Invia il comando di accensione led di pos monopattino
            Publish("Scooter/Scooter1/Cmd/Display", "Scooter Display OFF"); //Invia il comando di accensione display monopattino

        }
    }
}
