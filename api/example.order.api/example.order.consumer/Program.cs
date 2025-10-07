using Autofac.Extensions.DependencyInjection;
using example.consumer;

var builder = Host.CreateDefaultBuilder(args)
                   .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                   .ConfigureWebHostDefaults(webHostBuilder =>
                   {
                       webHostBuilder
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseIISIntegration()
                        .UseStartup<Startup>();
                   });

var app = builder.Build();
app.Run();