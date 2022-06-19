using GetInstrument_Job.Interface;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GetInstrument_Job.Services
{
    public class InstrumentServiceJob : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IMarketSummaryService _marketSummary;
        private readonly IStockSummaryService _stockSummaryService;
        private Timer _timer;
        private Timer _timer2;

        public InstrumentServiceJob(ILogger<InstrumentServiceJob> logger, IMarketSummaryService marketSummary, IStockSummaryService stockSummaryService)
        {
            _logger = logger;
            _marketSummary = marketSummary; 
            _stockSummaryService = stockSummaryService;    
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));
           // _timer2 = new Timer(PublishtoQueue, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            //Task.Delay(2000).Wait();    
            _logger.LogInformation("Service is running.");
            _marketSummary.InsertMarketSummaryAsync();
            _stockSummaryService.PushAlertRequest();
            _stockSummaryService.InsertStockSummaryAsync();

        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}