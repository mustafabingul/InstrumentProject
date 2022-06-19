using GetInstrument_Job.Interface;
using GetInstrument_Job.Services;
using Instrument_Infrastructure.DI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;
using Instrument_Application.Interface;
using StackExchange.Redis;
using Instrument_Infrastructure.Services;
using Instrument_Infrastructure.DAO;

namespace GetInstrument_Job
{
    public class Program
    {
        private IConfiguration _configuration;

        public Program(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static async Task Main(string[] args)
        {
        var hostBuilder = new HostBuilder()
            .ConfigureAppConfiguration((hostContext, configBuilder) =>
            {
                configBuilder.SetBasePath(Directory.GetCurrentDirectory());
                configBuilder.AddJsonFile("appsettings.json", optional: true);
                configBuilder.AddJsonFile(
                    $"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
                    optional: true);
                configBuilder.AddEnvironmentVariables();
            })
            .ConfigureLogging((hostContext, configLogging) =>
            {
                configLogging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
                configLogging.AddConsole();
            })
            .ConfigureServices((hostContext, services) =>
            {
                
                services.AddSingleton<IConnectionMultiplexer>(x =>
                 ConnectionMultiplexer.Connect(hostContext.Configuration.GetValue<string>("Redisconnection")));
                services.AddSingleton<IRedisCacheService, RedisCacheService>();
                services.AddSingleton<IMessageProducer, MessageProducer>();
                services.AddInfrastructure();
                
                
                services.AddScoped<IStockSummaryService, StockSummaryService>();
                services.AddTransient<IMarketSummaryService, MarketSummaryService>();
                services.AddScoped<IHostedService, InstrumentServiceJob>();
            });

          await hostBuilder.RunConsoleAsync();
        }
    }


}

