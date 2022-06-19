using Instrument_Domain.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstrumentMessageService.Services
{
    public class ConsumerService : IConsumerService, IDisposable
    {
        private readonly IModel _model;
        private readonly IConnection _connection;
        private ILogger<ConsumerService> _logger;
        private IEmailService _emailService;  
        public ConsumerService(IMessagingService messagingService, IEmailService emailService, ILogger<ConsumerService> logger)
        {
            _connection = messagingService.CreateChannel();
            _logger = logger;
            _emailService = emailService;
            _model = _connection.CreateModel();
            _model.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false);
            _model.ExchangeDeclare("UserExchange", ExchangeType.Fanout, durable: true, autoDelete: false);
            _model.QueueBind(_queueName, "UserExchange", string.Empty);
        }
        const string _queueName = "priceAlerts";

        public async Task ReadMessgaes()
        {
            var consumer = new AsyncEventingBasicConsumer(_model);
            consumer.Received += async (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                var text = System.Text.Encoding.UTF8.GetString(body);
                var messageAlert = JsonConvert.DeserializeObject<AlertRequest>(text);
                _logger.LogInformation("Recieved Message ::" + text);
                
                _emailService.SendEmail(messageAlert);
                await Task.CompletedTask;
                _model.BasicAck(ea.DeliveryTag, false);
            };
            _model.BasicConsume(_queueName, false, consumer);
            await Task.CompletedTask;
        }
        public void Dispose()
        {
            if (_model.IsOpen)
                _model.Close();
            if (_connection.IsOpen)
                _connection.Close();
        }
    }
}