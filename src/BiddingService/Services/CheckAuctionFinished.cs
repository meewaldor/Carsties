

using BiddingService.Models;
using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace BiddingService.Services
{
    public class CheckAuctionFinished : BackgroundService
    {
        private readonly ILogger<CheckAuctionFinished> _logger;
        private readonly IServiceProvider _services;
        public CheckAuctionFinished(ILogger<CheckAuctionFinished> logger, IServiceProvider services)
        {
            _logger = logger;
            _services = services;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting check for finished auctions.");
            stoppingToken.Register(() => _logger.LogInformation("==> Auction check is stopping"));

            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckAuction(stoppingToken);
                await Task.Delay(5000, stoppingToken);
            }
        }

        private async Task CheckAuction(CancellationToken stoppingToken)
        {
            var finishedAuctions = await DB.Find<Auction>()
                .Match(a => a.AuctionEnd <= DateTime.UtcNow)
                .Match(x => !x.Finished)
                .ExecuteAsync(stoppingToken);

            if (finishedAuctions.Count == 0) return;

            _logger.LogInformation("==> Found {count} finished auctions.", finishedAuctions.Count);

            using var scope = _services.CreateScope();
            var endpoint = scope.ServiceProvider.GetService<IPublishEndpoint>();

            foreach(var auction in finishedAuctions)
            {
                auction.Finished = true;
                await auction.SaveAsync(null,stoppingToken);
                var winningBid = await DB.Find<Bid>()
                    .Match(b => b.AuctionId == auction.ID)
                    .Match(b => b.BidStatus == BidStatus.Accepted)
                    .Sort(b => b.Ascending(s => s.Amount))
                    .ExecuteFirstAsync(stoppingToken);

                await endpoint.Publish(new AuctionFinished
                {
                    ItemSold = winningBid != null,
                    AuctionId = auction.ID.ToString(),
                    Winner = winningBid?.Bidder,
                    Amount = winningBid?.Amount,
                    Seller = auction.Seller,
                }, stoppingToken);
            }

            
        }
    }
}
