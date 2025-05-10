using BiddingService.Models;
using BiddingService.protos;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;

namespace BiddingService.Services
{
    public class GrpcAuctionClient
    {
        private readonly ILogger<GrpcAuctionClient> _logger;
        private readonly IConfiguration _config;
        public GrpcAuctionClient(ILogger<GrpcAuctionClient> logger, IConfiguration config)
        {
            _config = config;
            _logger = logger;
        }

        public Auction GetAuction (string id)
        {
            _logger.LogInformation("Calling Grpc service");
            var channel = GrpcChannel.ForAddress(_config["GrpcAuction"]);
            var client = new GrpcAuction.GrpcAuctionClient(channel);
            var request = new GetAuctionRequest { Id = id };

            try
            {
                var reply = client.GetAuction(request);
                var auction = new Auction
                {
                    ID = reply.Auction.Id,
                    AuctionEnd = DateTime.Parse(reply.Auction.AuctionEnd),
                    Seller = reply.Auction.Seller,
                    ReservePrice = reply.Auction.ReservePrice,
                };
                return auction;
            } catch(Exception ex)
            {
                _logger.LogError(ex, "Could not call Grpc server");
                return null;
            }
        }
    }
}
