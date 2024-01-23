using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Microsoft.AspNetCore.HttpLogging;
using OpenTelemetry.Instrumentation.AspNetCore;
using Bogus;
using otelSample.Controllers;


var builder = WebApplication.CreateBuilder(args);

//builder.Logging.AddFilter<OpenTelemetryLoggerProvider>("*", LogLevel.Information);
builder.Logging.AddOpenTelemetry(options =>
{
     options.IncludeFormattedMessage = true;
     options.IncludeScopes = true;

   var resource = ResourceBuilder.CreateDefault().AddService("otelSample");
   options.SetResourceBuilder(resource);
   options.AddConsoleExporter();
   options.AddOtlpExporter();

});

//Filter out instrumentation of the Prometheus scraping endpoint
builder.Services.Configure<AspNetCoreTraceInstrumentationOptions>(opt => 
{
   opt.Filter = ctx => ctx.Request.Path != "/metrics";   
});

//add trancing setUp
builder.Services.AddOpenTelemetry()
   .ConfigureResource(res => res.AddService(serviceName: builder.Environment.ApplicationName))
   .WithTracing(
      tracing => tracing
      .AddAspNetCoreInstrumentation()
      .AddHttpClientInstrumentation()
      .AddConsoleExporter()
      .AddOtlpExporter())

   //add trancing setUp
   .WithMetrics(metrics => metrics
      .AddAspNetCoreInstrumentation()
      .AddHttpClientInstrumentation()
      .AddRuntimeInstrumentation()
      
      .AddConsoleExporter()
      .AddOtlpExporter());

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpLogging(o => o.LoggingFields = HttpLoggingFields.All);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   app.UseSwagger();
   app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();



app.Run();
