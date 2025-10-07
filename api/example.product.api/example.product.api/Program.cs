using Autofac.Extensions.DependencyInjection;
using example.api;

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

//dotnet dev-certs https --trust