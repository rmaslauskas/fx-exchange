namespace Exchange.Application
{
    using Exchange.Application.Behaviors;
    using Exchange.Application.Factories;
    using Exchange.Domain.Contracts;
    using FluentValidation;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public static class ConfigureService
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
            services.AddValidatorsFromAssembly(typeof(ConfigureService).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient<ICurrencyPairFactory, CurrencyPairFactory>();

            return services;
        }
    }
}