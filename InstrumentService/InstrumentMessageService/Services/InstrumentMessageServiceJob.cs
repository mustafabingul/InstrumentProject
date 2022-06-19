using Instrument_Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InstrumentMessageService.Services
{
    public class InstrumentMessageServiceJob : BackgroundService
    {
        private readonly IConsumerService _consumerService;

        public InstrumentMessageServiceJob(IConsumerService consumerService)
        { 
            _consumerService = consumerService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _consumerService.ReadMessgaes();
        }
    }
}