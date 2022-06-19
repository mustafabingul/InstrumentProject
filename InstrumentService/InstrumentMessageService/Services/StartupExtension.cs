using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstrumentMessageService.Services
{
    public static class StartupExtension
    {
        public static void AddCommonService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMQConfiguration>(a => configuration.GetSection(nameof(RabbitMQConfiguration)).Bind(a));
            services.AddSingleton<IMessagingService, MessagingService>();
        }
    }    
}
