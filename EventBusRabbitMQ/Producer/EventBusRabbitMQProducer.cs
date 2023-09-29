using EventBusRabbitMQ.Events.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;
using System.Text;

namespace EventBusRabbitMQ.Producer
{
    public class EventBusRabbitMQProducer  //amacı bir tane event üretip queue ya bırakmak
    {
        private readonly IRabbitMQPersistentConnection _rabbitMQPersistentConnection;
        private readonly ILogger<EventBusRabbitMQProducer> _logger;
        private readonly int _retryCount;

        public EventBusRabbitMQProducer(IRabbitMQPersistentConnection rabbitMQPersistentConnection, ILogger<EventBusRabbitMQProducer> logger, int retryCount = 5)
        {
            _rabbitMQPersistentConnection = rabbitMQPersistentConnection;
            _logger = logger;
            _retryCount = retryCount;
        }

        public void Publish(string queueName, IEvent @event )  //event keyword ü derleyici için anlamlı bir ifade oldugu icin @ ifadesi koyuyoruz basına
        {
            if (!_rabbitMQPersistentConnection.IsConnected)
            {
                _rabbitMQPersistentConnection.TryConnect();
            }

            //kullandığımız policy ler connection veya iletişim sıkıntısınde tekrar denemesini istiyoruz bu yüzden policy lere ihtiyacımız var.
            var policy = RetryPolicy.Handle<SocketException>()
                                    .Or<BrokerUnreachableException>()
                                    .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                                    {
                                        _logger.LogWarning(ex, "RabbitMQ Client could not connect after {TimeOut}s ({ExpcetionMessage})", $"{time.TotalSeconds:n1}", ex.Message);
                                    });

            using(var channel = _rabbitMQPersistentConnection.CreateModel()) //createModel methodu ile IModel cinsinden nesne geliyor o nesne üzerinden queue işlemlerimizi yapıyoruz
            {
                channel.QueueDeclare(queueName, durable:false,exclusive:false,autoDelete:false,arguments:null);
                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);

                //kullandığımız policy ler connection veya iletişim sıkıntısınde tekrar denemesini istiyoruz bu yüzden policy lere ihtiyacımız var.
                policy.Execute(() =>
                {
                    IBasicProperties properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    properties.DeliveryMode = 2;

                    channel.ConfirmSelect();
                    channel.BasicPublish(
                          exchange: "",
                          routingKey: queueName,
                          mandatory: true,
                          basicProperties: properties,
                          body: body);
                    channel.WaitForConfirmsOrDie();

                    channel.BasicAcks += (sender, eventArgs) =>
                    {
                        Console.WriteLine("Sent RabbitMQ");
                        //implement ack handle
                    };

                });
            }



        }

    }
}
