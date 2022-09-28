using Microsoft.Extensions.DependencyInjection;
using Patite.Domain.Bus;
using Patite.Infra.Bus;

namespace Patite.Infra.IoC
{
    public class DependencyContainer
    {
        public static void RegisterService(IServiceCollection services)
        {
            //Domain Bus
            services.AddTransient<IEventBus, RabbitMqBus>();
        }
    }
}