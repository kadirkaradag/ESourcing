using AutoMapper;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Core;
using EventBusRabbitMQ.Events;
using MediatR;
using Newtonsoft.Json;
using Ordering.Application.Commands.OrderCreate;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace ESourcing.Order.Consumers
{
    public class EventBusOrderCreateConsumer
    {
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public EventBusOrderCreateConsumer(IRabbitMQPersistentConnection persistentConnection, IMediator mediator, IMapper mapper)
        {
            _persistentConnection = persistentConnection;
            _mediator = mediator;
            _mapper = mapper;
        }

        public void Consume()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var channel = _persistentConnection.CreateModel();
            channel.QueueDeclare(queue: EventBusConstants.OrderCreateQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);  //sourcing kısmında RabbitMQ da OrderCreateQueue'a mesaj bırakıyorduk, burada OrderCreateQueue'a gelen mesajları dinleyeceğiz.

            var consumer = new EventingBasicConsumer(channel); 

            consumer.Received += RecievedEvent; //gelen her mesajı RecievedEvent methoduna yolla

            channel.BasicConsume(queue: EventBusConstants.OrderCreateQueue, autoAck: true, consumer:consumer); //consume etmeye baslıyoruz

        }

        private async void RecievedEvent(object sender, BasicDeliverEventArgs e) //consume ettğimiz mesajlar buraya düşecek, mesajlar ile neler yapacağımızı belirttiğimiz kısım
        {
            var message = Encoding.UTF8.GetString(e.Body.Span); //mesaja ulastık
            var @event = JsonConvert.DeserializeObject<OrderCreateEvent>(message); //mesajı alıp deserialize edip  OrderCreateEvent e dönüğştürüp kullanıcaz

            if(e.RoutingKey == EventBusConstants.OrderCreateQueue)
            {
                var command = _mapper.Map<OrderCreateCommand>(@event); //aldığımız OrderCreateEvent tipini de OrderCreateCommand handler tipine dönüştürüyoruz

                command.CreatedAt = DateTime.Now;
                command.TotalPrice = @event.Quantity * @event.Price;
                command.UnitPrice = @event.Price;

                var result = await _mediator.Send(command);  // burada da OrderCreateCommand a girildiğinde gidip bunu handle eden mediatr Handler ı buluyor yani OrderCreateHandler class'ına gidiyor orada da zaten db ye order kaydı atıyor..

            }
        }

        public void Disconnect() // connection bırakmamak icin dispose işlemi
        {
            _persistentConnection.Dispose();
        }

    }
}
