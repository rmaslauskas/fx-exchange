namespace Exchange.Application
{
    using System.IO;
    using Exchange.Domain;
    using Exchange.Infrastructure;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    internal class Program
    {
        public static void Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            var serviceProvider = services.BuildServiceProvider();

            serviceProvider.GetService<App>().Run(args);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", false)
               .Build();


            services.AddSingleton<IConfiguration>(configuration);
            services.AddTransient<App>();
            services.AddApplicationServices();
            services.AddInfrastructureModule(configuration);
        }
    }
}