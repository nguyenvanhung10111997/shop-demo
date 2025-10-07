using Autofac;
using Autofac.Extensions.DependencyInjection;
using example.consumer.Configurations;
using example.infrastructure.ContainerManager;
using example.order.infrastructure.Configurations;

namespace example.consumer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AppSettingRegister.Binding(configuration);
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDatabase();
            services.AddRepositories();
            services.AddControllers();
            services.ConfigureMassTransit();
        }

        //this method is called by the runtime. when use register use AutofacServiceProviderFactory in function startup in program.cs
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterInfrastructureDependeny();
        }

        public void Configure(IApplicationBuilder app)
        {
            Engine.ContainerManager = new ContainerManager(app.ApplicationServices.GetAutofacRoot());
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(x => x.MapControllers());
        }
    }
}