using Amazon.SimpleNotificationService;
using Autofac;
using example.domain.Interfaces;
using example.infrastructure;
using example.infrastructure.Configurations;
using example.infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace example.order.infrastructure.Configurations;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddDbContext<ExampleDbContext>(options =>
        {
            options.UseSqlServer(ApiConfig.Connection.DefaultConnectionString);
            options.UseLazyLoadingProxies();
        });

        services.AddScoped<ExampleDbContext>();
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }

    public static void RegisterInfrastructureDependeny(this ContainerBuilder builder)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(x => x.GetName().Name.Contains("infrastructure"));

        foreach (var assembly in assemblies)
        {
            if (assembly != null)
            {
                builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces().InstancePerLifetimeScope();
            }
        }

        var entryAssembly = Assembly.GetEntryAssembly();
        builder.RegisterAssemblyTypes(entryAssembly).AsImplementedInterfaces().InstancePerLifetimeScope();

        builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
    }

    public static IServiceCollection ConfigureMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            var executingAssembly = Assembly.GetEntryAssembly();
            x.AddConsumers(executingAssembly);

            x.UsingAmazonSqs((context, cfg) =>
            {
                cfg.Host(new Uri(ApiConfig.Providers.AmazonSQS.Host), h =>
                {
                    h.AccessKey(ApiConfig.Providers.AmazonSQS.AccessKey);
                    h.SecretKey(ApiConfig.Providers.AmazonSQS.SecretKey);
                    h.Config(new AmazonSimpleNotificationServiceConfig { ServiceURL = ApiConfig.Providers.AmazonSQS.ServiceURL });
                    h.Config(new Amazon.SQS.AmazonSQSConfig { ServiceURL = ApiConfig.Providers.AmazonSQS.ServiceURL });
                });

                var consumerType = executingAssembly.GetType(ApiConfig.Providers.AmazonSQS.ConsumerName);

                cfg.ReceiveEndpoint(ApiConfig.Providers.AmazonSQS.QueueName, e =>
                {
                    //e.Subscribe(ApiConfig.Providers.AmazonSQS.TopicName);

                    if (consumerType != null)
                    {
                        e.ConfigureConsumer(context, consumerType);
                    }
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}