using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Entities;
using System.Text.Json;

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

            var count = await DB.CountAsync<Item>();
            if(count == 0)
            {
                Console.WriteLine("Seeding database with initial data...");
                var itemData = await File.ReadAllTextAsync("Data/auctions.json");
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var items = JsonSerializer.Deserialize<List<Item>>(itemData, options);  
                await DB.SaveAsync(items);
            }
        }
    }
}
