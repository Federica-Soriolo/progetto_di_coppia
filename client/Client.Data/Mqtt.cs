using Newtonsoft.Json;
using System;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Client.Data
{
    public class Mqtt: IMqtt
    {
        private static MqttClient client;
        private readonly string BrokerAddress = "127.0.0.1";
        public bool Race { get; set; }
        public bool Scooter { get; set; }

        public Mqtt(string clientId)
        {
            client = new MqttClient(BrokerAddress);
            client.Connect(clientId, null, null, false, 2, true, "Scooter/Scooter1/LastWillTopic", "Unexpected exit", true, 60);
            client.MqttMsgPublishReceived += PublishReceived;
            Race = false;
            Scooter = false;
        }

        public void Subscribe(string topic)
        {
            if (topic != "")
            {

                client.Subscribe(new string[] { topic }, new byte[] { 2 });
                Console.WriteLine($"Ok subscribed topic: {topic}");
            }
            else
            {
                Console.WriteLine("Invalid topic.");
            }
        }

        public void Publish(string topic, string message)
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

        public void PublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string ReceivedMessage = Encoding.UTF8.GetString(e.Message);

            //Console.WriteLine($"Message: {ReceivedMessage}");

            if (ReceivedMessage == "Start race") Race = true;
            if (ReceivedMessage == "Stop race") Race = false;

            if (ReceivedMessage == "Scooter ON") Scooter = true;
            if (ReceivedMessage == "Scooter OFF") Scooter = false;
        }

    }
}
