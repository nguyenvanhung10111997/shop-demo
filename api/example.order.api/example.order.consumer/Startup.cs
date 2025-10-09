using Amazon.SimpleNotificationService;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using example.consumer.Configurations;
using example.infrastructure.Configurations;
using example.infrastructure.ContainerManager;
using example.order.consumer.Consumers;
using example.order.infrastructure.Configurations;
using example.order.infrastructure.Messages;
using MassTransit;

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
            ConfigureMassTransit(services);      
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

        private void ConfigureMassTransit(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<CreateOrderConsumer>();

                x.UsingAmazonSqs((context, cfg) =>
                {
                    cfg.Host(new Uri(ApiConfig.Providers.AmazonSQS.Host), h =>
                    {
                        h.AccessKey(ApiConfig.Providers.AmazonSQS.AccessKey);
                        h.SecretKey(ApiConfig.Providers.AmazonSQS.SecretKey);
                        h.Config(new AmazonSimpleNotificationServiceConfig { ServiceURL = ApiConfig.Providers.AmazonSQS.ServiceURL });
                        h.Config(new Amazon.SQS.AmazonSQSConfig { ServiceURL = ApiConfig.Providers.AmazonSQS.ServiceURL });
                    });

                    cfg.ReceiveEndpoint(ApiConfig.Providers.AmazonSQS.QueueNames.CreateOrderQueue, e =>
                    {
                        e.ConfigureConsumer<CreateOrderConsumer>(context);
                    });

                    cfg.Message<OrderCreateMessage>(x =>
                    {
                        x.SetEntityName(ApiConfig.Providers.AmazonSQS.QueueNames.CreateOrderQueue);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}