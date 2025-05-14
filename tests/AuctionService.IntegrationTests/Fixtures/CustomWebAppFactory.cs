using AuctionService.Data;
using AuctionService.IntegrationTests.Utils;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using Testcontainers.PostgreSql;

namespace AuctionService.IntegrationTests.Fixtures
{
    // Custom WebApplicationFactory to override app configuration for integration testing
    public class CustomWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        // Create a PostgreSQL container instance using Testcontainers
        private PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder().Build();
        public async Task InitializeAsync()
        {
            await _postgreSqlContainer.StartAsync();
        }
        // Configures the test web host environment and services
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                
                services.RemoveDbContext<AuctionDbContext>();
                
                // Register DbContext using the PostgreSQL test container's connection string
                services.AddDbContext<AuctionDbContext>(options =>
                {
                    options.UseNpgsql(_postgreSqlContainer.GetConnectionString());
                });

                // Add MassTransit test harness to test message consumers without needing a real message broker
                services.AddMassTransitTestHarness();

                services.EnsureCreated<AuctionDbContext>();

            });
        }

        //Disposes of the PostgreSQL container.
       Task IAsyncLifetime.DisposeAsync() => _postgreSqlContainer.DisposeAsync().AsTask();
    }
}
