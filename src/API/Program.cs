using System.Net;
using Cuby.Data;
using Cuby.Middlewares;
using Cuby.Services.impl;
using Cuby.Services.interfaces;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Cuby.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddDbContext<RequestDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnectionString")));

            builder.Services.AddTransient<IRequestService, RequestService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddOpenApi();

            // Configuration OpenTelemetry
            var openTelemetry = builder.Services.AddOpenTelemetry();

            openTelemetry.ConfigureResource(resource =>
            {
                resource.AddService(serviceName: "Cuby-capture-API", serviceVersion: "1.0.0")
                        .AddAttributes(new Dictionary<string, object>
                        {
                            { "executionServer", Dns.GetHostName() },
                            { "project", "Cuby-capture-API" },
                            { "serverName", Environment.MachineName }
                        });
            });

            openTelemetry.WithTracing(tracing => tracing
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Cuby-capture-API"))
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri(builder.Configuration["OpenTelemetryExporterUrl"]!);
                    options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                }));

            openTelemetry.WithMetrics(metrics => metrics
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Cuby-capture-API"))
                .AddAspNetCoreInstrumentation()
                .AddRuntimeInstrumentation()
                .AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri(builder.Configuration["OpenTelemetryExporterUrl"]!);
                    options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                }));

            builder.Logging.AddOpenTelemetry(logging =>
            {
                logging.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Cuby-capture-API"));
                logging.IncludeFormattedMessage = true;
                logging.IncludeScopes = true;
                logging.ParseStateValues = true;
                logging.AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri(builder.Configuration["OpenTelemetryExporterUrl"]!);
                    options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                });
            }

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<RequestDbContext>();
                context.Database.EnsureCreated();
            }

            app.UseMiddleware<RequestLoggingMiddleware>();

            app.UseHttpsRedirection();

            app.MapControllers();

            app.Run();
        }
    }
}
