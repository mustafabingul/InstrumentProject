using Instrument_Application.Interface;
using Instrument_Domain.Models;
using Instrument_Infrastructure.DI;
using InstrumentMessageService.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace InstrumentMessageService
{
    public class Program
    {
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
                    services.AddCommonService(hostContext.Configuration);
                    services.AddTransient<IEmailService, EmailService>();
                    services.AddSingleton<IConsumerService, ConsumerService>();

                    services.AddHostedService<InstrumentMessageServiceJob>();
                });

            await hostBuilder.RunConsoleAsync();            
        }
    }
}
