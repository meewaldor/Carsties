

using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.IntegrationTests.Fixtures;
using AuctionService.IntegrationTests.Utils;
using Contracts;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace AuctionService.IntegrationTests
{
    [Collection("Shared collection")]
    public class AuctionBusTests : IAsyncLifetime
    {
        private readonly CustomWebAppFactory _factory;
        private readonly HttpClient _httpClient;
        private readonly ITestHarness _testHarness;
        public AuctionBusTests(CustomWebAppFactory factory)
        {
            _factory = factory;
            _httpClient = factory.CreateClient();
            _testHarness = factory.Services.GetTestHarness();
        }

        [Fact]
        public async Task CreateAuction_WithValidObject_ShouldPublishAuctionCreated()
        {
            // arrange
            var auction = GetAuctionForCreate();
            _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));

            //act
            var response = await _httpClient.PostAsJsonAsync($"api/auctions", auction);

            // assert
            response.EnsureSuccessStatusCode();
            Assert.True(await _testHarness.Published.Any<AuctionCreated>());
        }

        // This method runs before any test in this class is executed
        public Task InitializeAsync() => Task.CompletedTask;

        // This method runs after all tests in the class have completed
        // It resets the database to a clean state
        public Task DisposeAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();
            DbHelper.ReInitDbForTests(db);
            return Task.CompletedTask;
        }
        private CreateAuctionDto GetAuctionForCreate()
        {
            return new CreateAuctionDto
            {
                Make = "test",
                Model = "testModel",
                ImageUrl = "testImageUrl",
                Color = "testColor",
                Mileage = 10,
                Year = 2020,
                ReservePrice = 10,
            };
        }
    }
}
