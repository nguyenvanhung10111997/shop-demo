using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Load Ocelot configuration
builder.Configuration
    .AddOcelot(builder.Environment);

// Add Ocelot services
builder.Services.AddOcelot(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .WithOrigins("*")    // Allow all domains
            .WithHeaders("*")    // Allow all HTTP methods
            .WithMethods("*");   // Allow all headers
    });
});

var app = builder.Build();

// Optionally add middleware before Ocelot
app.UseRouting();
app.UseCors("AllowAll");

// Example: Add simple logging middleware
app.Use(async (context, next) =>
{
    Console.WriteLine($"[{DateTime.Now}] {context.Request.Method} {context.Request.Path}");
    await next.Invoke();
});

// Run Ocelot
await app.UseOcelot();

app.Run();
