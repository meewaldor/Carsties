using AuctionService.Data;
using AuctionService.protos;
using Grpc.Core;

namespace AuctionService.Services
{
    public class GrpcAuctionService : GrpcAuction.GrpcAuctionBase
    {
        private readonly AuctionDbContext _dbContext;
        public GrpcAuctionService(AuctionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<GrpcAuctionResponse> GetAuction (GetAuctionRequest request, ServerCallContext context)
        {
            Console.WriteLine("Receid Grpc request for auction");
            var auction = await _dbContext.Auctions.FindAsync(Guid.Parse(request.Id));
            if (auction == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Auction not found"));
            }

            var response = new GrpcAuctionResponse
            {
                Auction = new GrpcAuctionModel
                {
                    AuctionEnd = auction.AuctionEnd.ToString(""),
                    Id = auction.Id.ToString(),
                    ReservePrice = auction.ReservePrice,
                    Seller = auction.Seller,
                }
            };

            return response;
        }
    }
}
