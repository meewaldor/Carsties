using Contracts;
using MassTransit;
using MongoDB.Entities;
using Polly;
using SearchService.Entities;

namespace SearchService.Consumers
{
    public class BidPlacedConsumer : IConsumer<BidPlace>
    {
        public async Task Consume(ConsumeContext<BidPlace> context)
        {
            Console.WriteLine("--> Consuming Bid Placed");
            var auction = await DB.Find<Item>().OneAsync(context.Message.AuctionId);

            if (context.Message.BidStatus.Contains("Accepted") && context.Message.Amount > auction.CurrentHighBid)
            {
                auction.CurrentHighBid = context.Message.Amount;
                await auction.SaveAsync();
            }
        }
    }
}
