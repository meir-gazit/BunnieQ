using MediatR;
using Patite.Domain.Bus;
using Patite.Domain.Commands;
using Patite.Domain.Events;
using System.Collections.Generic;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;
using System.Linq;
using RabbitMQ.Client.Events;

namespace Patite.Infra.Bus
{
    public sealed class RabbitMqBus : IEventBus
    {
        private readonly IMediator _mediator;
        private readonly Dictionary<string, List<Type>> _Handlers;
        private readonly List<Type> _eventTypes;
        private readonly string host = "localhost";

        public RabbitMqBus(IMediator mediator)
        {
            _mediator = mediator;
            _Handlers = new Dictionary<string, List<Type>>();
            _eventTypes = new List<Type>();
        }

        public Task SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }

        public void Publish<T>(T @event) where T : Event
        {
            var factory = new ConnectionFactory() { HostName = host };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            { 
                var eventName = @event.GetType().Name;
                channel.QueueDeclare(eventName,false, false, false, null);
                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("", eventName, null, body);
            }

        }

        public void Subscribe<T, THandler>()
            where T : Event
            where THandler : IEventHandler<T>
        {
            var eventName = typeof(T).Name;
            var handlerType = typeof(THandler);

            if (!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }

            if (!_Handlers.ContainsKey(eventName))
            {
                _Handlers.Add(eventName, new List<Type>());
            }

            if (_Handlers[eventName].Any(s => s.GetType() == handlerType))
            {
                throw new ArgumentException($"Handler Type {handlerType.Name} already register for '{eventName}'.", nameof(handlerType));
            }

            _Handlers[eventName].Add(handlerType);
            StartBasicConsume<T>();
        }

        private void StartBasicConsume<T>() where T : Event
        {
            var factory = new ConnectionFactory()
            {
                HostName = host,
                DispatchConsumersAsync = true,
            };

            var connection = factory.CreateConnection();    
            var channel = connection.CreateModel();

            var eventName = typeof(T).Name;

            channel.QueueDeclare(eventName, false, false, false, null);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.Received += Consumer_Received;

            channel.BasicConsume(eventName, true, consumer);
                    
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var eventName = e.RoutingKey;
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            try
            {
                await ProcessEvent(eventName, message).ConfigureAwait(false);
            }
            catch (Exception ex)
            {

            }
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            var subscriptions = _Handlers[eventName];
            foreach (var subscription in subscriptions)
            {
                var handler = Activator.CreateInstance(subscription);
                if (handler == null) continue;
                var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);
                var @event = JsonConvert.DeserializeObject(message, eventType);
                var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { @event });
            }

        }
    }
}