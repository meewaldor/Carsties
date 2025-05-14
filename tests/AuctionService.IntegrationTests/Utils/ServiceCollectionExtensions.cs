

using AuctionService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionService.IntegrationTests.Utils
{
    public static class ServiceCollectionExtensions
    {
        // Remove the existing DbContext configuration
        public static void RemoveDbContext<T>(this IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(d =>
            d.ServiceType == typeof(DbContextOptions<AuctionDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
        }

        // Build service provider and apply any EF Core migrations to prepare the schema
        public static void EnsureCreated<T>(this IServiceCollection services)
        {           
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AuctionDbContext>();

            db.Database.Migrate();
            DbHelper.InitDbForTests(db);
        }
    }
}
