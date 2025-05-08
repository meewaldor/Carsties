using AuctionService.Data;
using Contracts;
using MassTransit;

namespace AuctionService.Consumers
{
    public class BidPlaceConsumer : IConsumer<BidPlace>
    {
        private readonly AuctionDbContext _context;
        public BidPlaceConsumer(AuctionDbContext context)
        {
            _context = context;
        }
        public async Task Consume(ConsumeContext<BidPlace> context)
        {
            Console.WriteLine("--> Consuming bid placed");
            var auction = await _context.Auctions.FindAsync(Guid.Parse(context.Message.AuctionId));

            if (auction.CurrentHighBid == null || context.Message.BidStatus.Contains("Accepted") && context.Message.Amount > auction.CurrentHighBid)
            {
                auction.CurrentHighBid = context.Message.Amount;
                await _context.SaveChangesAsync();

            }
        }
    }
}
