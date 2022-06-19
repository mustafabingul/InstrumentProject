using Instrument_Application.Interface;
using Instrument_Infrastructure.DAO;
using Instrument_Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instrument_Infrastructure.DI
{
    public class Start
    {
        public Start(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
   
          
           
            services.AddSingleton<IConnectionMultiplexer>(x =>
                ConnectionMultiplexer.Connect(Configuration.GetValue<string>("Redisconnection")));
            services.AddTransient<IMarketSummaryDAO, MarketSummaryDAO>();

        }
    }
}
