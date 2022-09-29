using Microsoft.Extensions.DependencyInjection;
using Patite.Domain.Bus;
using Patite.Infra.Bus;
using Patite.Microservice.Banking.Application.Interfaces;
using Patite.Microservice.Banking.Application.Services;
using Patite.Microservice.Banking.Data.Context;
using Patite.Microservice.Banking.Data.Repository;
using Patite.Microservice.Banking.Domain.Interfaces;

namespace Patite.Infra.IoC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            //Domain Bus
            services.AddTransient<IEventBus, RabbitMqBus>();

            //Application Service
            services.AddTransient<IAccountService, AccountService>();

            //Data
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<BankingDBContext>(); 
        }
    }
}