using Autofac;
using example.domain.Interfaces;
using example.infrastructure;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace example.service.Configurations
{
    public static partial class ServiceCollectionExtensions
    {
        public static void RegisterServiceDependeny(this ContainerBuilder builder)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.GetName().Name.Contains("service"));

            foreach (var assembly in assemblies)
            {
                if (assembly != null)
                {
                    builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces().InstancePerLifetimeScope();
                }
            }

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
        }

        public static void AddMediator(this IServiceCollection services)
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name.Contains("service"));

            if (assembly != null)
            {
                services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));

                services.AddValidatorsFromAssembly(assembly);
            }
        }
    }
}
