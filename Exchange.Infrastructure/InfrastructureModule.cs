using Exchange.Domain.Contracts;
using Exchange.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Infrastructure
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructureModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient("ExchangeAPI", client =>
            {
                client.BaseAddress = new Uri(configuration.GetValue<string>("ExchangeRatesAPI") ?? string.Empty);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            services.AddTransient<IExchangeRatesService, ExchangeRatesService>();

            return services;
        }
    }
}
