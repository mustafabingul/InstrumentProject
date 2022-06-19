using Instrument_Application.Interface;
using Instrument_Infrastructure.DAO;
using Instrument_Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instrument_Infrastructure.DI
{
    public static class DependencyInjection
    {
        
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            //DAO Registration
            services.AddSingleton<Start>();
            services.AddSingleton<ICacheService, CacheService>();
            services.AddTransient<IMarketSummaryDAO, MarketSummaryDAO>();
            services.AddTransient<IStockSummaryDAO, StockSummaryDAO>();

         
          
            return services;
        }
    }
}

