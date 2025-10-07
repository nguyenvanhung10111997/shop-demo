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
}