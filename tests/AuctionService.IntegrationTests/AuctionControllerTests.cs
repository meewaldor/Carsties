
using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.IntegrationTests.Fixtures;
using AuctionService.IntegrationTests.Utils;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace AuctionService.IntegrationTests
{
    public class AuctionControllerTests : IClassFixture<CustomWebAppFactory>, IAsyncLifetime
    {
        private readonly CustomWebAppFactory _factory;
        private readonly HttpClient _httpClient;
        private const string GT_ID = "afbee524-5972-4075-8800-7d1f9d7b0a0c";
        public AuctionControllerTests(CustomWebAppFactory factory)
        {
            _factory = factory;
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task GetAuctions_ShouldReturn3Auctions()
        {
            // Arrange

            // act
            var response = await _httpClient.GetFromJsonAsync<List<AuctionDto>>("api/auctions");

            // assert
            Assert.Equal(3, response.Count); 
        }
        [Fact]
        public async Task GetAuctionById_WithValidId_ShouldReturnAuction()
        {
            // Arrange

            // act
            var response = await _httpClient.GetFromJsonAsync<AuctionDto>($"api/auctions/{GT_ID}");

            // assert
            Assert.Equal("GT", response.Model);
        }
        [Fact]
        public async Task GetAuctionById_WithInValidId_ShouldReturn404()
        {
            // Arrange

            // act
            var response = await _httpClient.GetAsync($"api/auctions/{Guid.NewGuid()}");

            // assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public async Task GetAuctionById_WithInValidGuid_ShouldReturn400()
        {
            // Arrange

            // act
            var response = await _httpClient.GetAsync($"api/auctions/notaguid");

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task CreateAuction_WithNoAuth_ShouldReturn401()
        {
            // Arrange
            var auction = new CreateAuctionDto { Make = "test" };
            // act
            var response = await _httpClient.PostAsJsonAsync($"api/auctions", auction);

            // assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        [Fact]
        public async Task CreateAuction_WithAuth_ShouldReturn201()
        {
            // Arrange
            var auction = GetAuctionForCreate();
            _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));
            // act
            var response = await _httpClient.PostAsJsonAsync($"api/auctions", auction);

            // assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var createdAuction = await response.Content.ReadFromJsonAsync<AuctionDto>();
            Assert.Equal("bob", createdAuction.Seller);
        }

        [Fact]
        public async Task CreateAuction_WithInvalidCreateAuctionDto_ShouldReturn400()
        {
            // arrange
            var auction = GetAuctionForCreate();
            auction.Make = null;
            _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));

            // act
            var response = await _httpClient.PostAsJsonAsync($"api/auctions", auction);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateAuction_WithValidUpdateDtoAndUser_ShouldReturn200()
        {
            // arrange
            var updateAuction = new UpdateAuctionDto { Make = "updated" };
            _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));

            // act
            var response = await _httpClient.PutAsJsonAsync($"api/auctions/{GT_ID}", updateAuction);

            // assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task UpdateAuction_WithValidUpdateDtoAndInvalidUser_ShouldReturn403()
        {
            // arrange 
            var updateAuction = new UpdateAuctionDto { Make = "updated" };
            _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("notbob"));

            // act
            var response = await _httpClient.PutAsJsonAsync($"api/auctions/{GT_ID}", updateAuction);

            // assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
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
