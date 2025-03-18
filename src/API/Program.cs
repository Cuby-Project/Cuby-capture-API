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

            // Configure PostgreSQL connection
            builder.Services.AddDbContext<RequestDbContext>(options =>
                    options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnectionString")));
            builder.Services.AddTransient<IRequestService, RequestService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            OpenTelemetryBuilder openTelemetry = builder.Services.AddOpenTelemetry();

            openTelemetry.ConfigureResource(ressource =>
            {
                ressource.AddService(serviceName: "Cuby-capture-API",
                                     serviceVersion: "1.0.0")
                         .AddAttributes(new Dictionary<string, object>()
                         {
                             ["executionServer"] = Dns.GetHostName(),
                             ["project"] = "Cuby-capture-API"
                         });
                ressource.AddAttributes(
                [
                    new KeyValuePair<string, object>("serverName", Environment.MachineName)
                ]);
            });
            openTelemetry.WithTracing(tracing => tracing
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("CubyCaptureAPI"))
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri(builder.Configuration["OpenTelemetryExporterUrl"]!);
                    options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                }));
            openTelemetry.WithMetrics(metrics => metrics
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("CubyCaptureAPI"))
                .AddAspNetCoreInstrumentation()
                .AddRuntimeInstrumentation()
                .AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri(builder.Configuration["OpenTelemetryExporterUrl"]!);
                    options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                }));

            builder.Logging.AddOpenTelemetry(logging =>
            {
                logging.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("CubyCaptureAPI"));
                logging.AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri(builder.Configuration["OpenTelemetryExporterUrl"]!);
                    options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                });
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                });
            }
            // we retireve the db context and apply the migrations
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<RequestDbContext>();
                context.Database.EnsureCreated();
            }

            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
