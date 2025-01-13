namespace Exchange.Domain
{
    using Exchange.Domain.Behaviors;
    using Exchange.Domain.Factories;
    using Exchange.Domain.Factories.Contracts;
    using FluentValidation;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public static class DomainModule
    {
        public static IServiceCollection AddDomainModule(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
            services.AddValidatorsFromAssembly(typeof(DomainModule).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient<ICurrencyPairFactory, CurrencyPairFactory>();

            return services;
        }
    }
}