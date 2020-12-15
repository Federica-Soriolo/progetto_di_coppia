using System;
using MQTTnet;
using MQTTnet.Client;
using System.Text;

namespace MqttTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var factory = new MqttFactory();
            IMqttClient mqttClient = factory.CreateMqttClient();

            IMqttClientOptions options = new MqttClientOptionsBuilder()
                .WithClientId("xxx123")
                .WithTcpServer("test.mosquitto.org")
                .WithCleanSession()
                .Build();

            connect(mqttClient, options);

            

            mqttClient.Connected += async (s, e) =>
            {
                write(mqttClient);

                /*Console.WriteLine("### CONNECTED WITH SERVER ###");

                // Subscribe to a topic
                await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("test/sv").Build());

                Console.WriteLine("### SUBSCRIBED ###");*/
            };

            /*while (false)
            {

                mqttClient.ApplicationMessageReceived += (s, e) =>
                {
                    Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
                    Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
                    Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                    Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                    Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
                    Console.WriteLine();
                };

            }*/

            Console.ReadLine();



        }

        static public async void write(IMqttClient mqttClient)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("test/sv")
                .WithPayload("Hello World")
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

                await mqttClient.PublishAsync(message);
        }

        static public async void connect(IMqttClient mqttClient, IMqttClientOptions options)
        {
            await mqttClient.ConnectAsync(options);
        }
    }
}
