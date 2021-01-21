using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;

namespace Client.Data
{
    public class Amqp : IAmqp
    {

        private string queueName;
        /**
         * Compatibile con vers. 0.9.1 (RabbitMQ)
         */
        public Amqp(string queue)
        {
            this.queueName = queue;
        }

        /*
         * Possibili refactoring:
         * - riutilizzare la stessa connessione per ogni invio, quindi aprirla all'avvio dell'applicazione e chiuderla al termine
         * - gestire situazioni d'errore: cosa accade se il broker non è raggiungibile?
         */
        public void Send(string message, string routingKey)
        {
            var factory = new ConnectionFactory()
            {
                Uri = new Uri("amqps://lhjvzajb:Tsu7NLVlJsIjtesiP-28UdzMMuEXif9l@bonobo.rmq.cloudamqp.com/lhjvzajb")
            };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "topic_logs",
                                    type: "topic");

                    
                    
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "topic_logs",
                                         routingKey: routingKey,
                                         basicProperties: null,
                                         body: body);
                    Console.WriteLine(" [x] Sent '{0}':'{1}'", routingKey, message);
                }
            }

        }
    }
}
