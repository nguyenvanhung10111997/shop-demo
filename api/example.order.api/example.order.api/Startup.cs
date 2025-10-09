using Amazon.SimpleNotificationService;
using Autofac;
using example.api.Configurations;
using example.infrastructure.Configurations;
using example.order.api.Middleware;
using example.order.infrastructure.Configurations;
using example.order.infrastructure.Messages;
using example.service.Configurations;
using MassTransit;
using MediatR;

namespace example.api
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
            services.AddMediator();
            services.AddControllers();
            ConfigureMassTransit(services);

            // Register the ValidationBehavior as a MediatR pipeline behavior
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy
                        .WithOrigins("*")    // Allow all domains
                        .WithHeaders("*")    // Allow all HTTP methods
                        .WithMethods("*");   // Allow all headers
                });
            });
        }
        //this method is called by the runtime. when use register use AutofacServiceProviderFactory in function startup in program.cs
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterInfrastructureDependeny();
            builder.RegisterServiceDependeny();
        }
        public void Configure(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors("AllowAll");

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(x => x.MapControllers());
        }

        private void ConfigureMassTransit(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumers(typeof(Program).Assembly);

                x.UsingAmazonSqs((context, cfg) =>
                {
                    cfg.Host(new Uri(ApiConfig.Providers.AmazonSQS.Host), h =>
                    {
                        h.AccessKey(ApiConfig.Providers.AmazonSQS.AccessKey);
                        h.SecretKey(ApiConfig.Providers.AmazonSQS.SecretKey);
                        h.Config(new AmazonSimpleNotificationServiceConfig { ServiceURL = ApiConfig.Providers.AmazonSQS.ServiceURL });
                        h.Config(new Amazon.SQS.AmazonSQSConfig { ServiceURL = ApiConfig.Providers.AmazonSQS.ServiceURL });
                    });

                    // ✅ Common FIFO topic attributes
                    var fifoAttributes = new Dictionary<string, string>
                    {
                        ["FifoTopic"] = "true",
                        ["ContentBasedDeduplication"] = "true"
                    };

                    // ✅ Configure message entities
                    ConfigureMessage<OrderCreateMessage>(cfg, ApiConfig.Providers.AmazonSQS.QueueNames.CreateOrderQueue, fifoAttributes);
                    ConfigureMessage<OrderUpdateMessage>(cfg, ApiConfig.Providers.AmazonSQS.QueueNames.UpdateOrderQueue, fifoAttributes);

                    // ✅ Configure all consumers automatically
                    cfg.ConfigureEndpoints(context);
                });
            });
        }

        /// <summary>
        /// Configures a message type for FIFO SQS/SNS integration
        /// </summary>
        private static void ConfigureMessage<TMessage>(
            IAmazonSqsBusFactoryConfigurator cfg,
            string queueName,
            Dictionary<string, string> fifoAttributes)
            where TMessage : class
        {
            // Ensure queue name ends with .fifo
            var fifoQueueName = queueName.EndsWith(".fifo", StringComparison.OrdinalIgnoreCase)
                ? queueName
                : $"{queueName}.fifo";

            // Configure entity (queue or topic)
            cfg.Message<TMessage>(x => x.SetEntityName(fifoQueueName));

            // Configure FIFO topic attributes
            cfg.Publish<TMessage>(x =>
            {
                foreach (var attr in fifoAttributes)
                    x.TopicAttributes[attr.Key] = attr.Value;
            });
        }
    }
}
