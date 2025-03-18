using System.Data.Entity;
using System.Net;
using Cuby.API;
using Cuby.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Cuby.Tests.Integration
{
    [TestClass]
    public sealed class IntegrationControllerTests
    {
        private static WebApplicationFactory<Program> _factory;

        private static HttpClient _client;

        [TestInitialize]
        public void TestInit()
        {
            _factory = new CustomWebApplicationFactory<Program>();
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("http://localhost")
            });
        }

        [TestMethod]
        public async Task InitSolveRequest_ShouldReturnRequestId()
        {
            // Act
            HttpResponseMessage response = await _client.GetAsync("/api/init");

            // Assert
            response.EnsureSuccessStatusCode();
            string requestId = await response.Content.ReadAsStringAsync();
            Assert.IsFalse(string.IsNullOrEmpty(requestId));
        }
    }

    public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            DbContextOptions<RequestDbContext> options = new DbContextOptionsBuilder<RequestDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"ConnectionStrings:RequestDb", "TestDatabase"},
                    {"OpenTelemetryExporterUrl", "http://localhost/"},
                })
                .Build();

            builder.ConfigureServices(services =>
            {

                services.Replace(ServiceDescriptor.Singleton(new RequestDbContext(options)));
            });
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddConfiguration(configuration);
            });
        }
    }
}