using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Entities;
using SearchService.Services;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace SearchService.Data
{
    public class DbInitializer
    {
        public static async Task InitDb(WebApplication app)
        {
            // Initialize MongoDB database connection with the name "SearchDb".
            // The connection string is retrieved from the app configuration (appsettings.json or environment).
            await DB.InitAsync("SearchDb", MongoClientSettings.
                FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));
            // Create a text index on the Item collection to support full-text search on Make, Model, and Color fields.
            await DB.Index<Item>()
                .Key(x => x.Make, KeyType.Text)
                .Key(x => x.Model, KeyType.Text)
                .Key(x => x.Color, KeyType.Text)
                .CreateAsync();

            using var scope = app.Services.CreateScope();
            var httpClient = scope.ServiceProvider.GetRequiredService<AuctionSvcHttpClient>();
            //Call the method to fetch updated items from the Auction Service API.
            var items = await httpClient.GetItemsForSearchDb();
            Console.WriteLine($"Items count: {items.Count}");
            if(items.Count > 0) await DB.SaveAsync(items);
        }
    }
}
