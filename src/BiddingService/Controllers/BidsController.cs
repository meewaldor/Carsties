using BiddingService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace BiddingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BidsController : ControllerBase
    {
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Bid>> PlaceBid(string auctionId, int amount)
        {
            var auction = await DB.Find<Auction>().OneAsync(auctionId);
            if(auction == null)
            {
                return NotFound("Auction not found");
            }
            if(auction.Seller == User.Identity.Name)
            {
                return BadRequest("You cannot bid on your own auction");
            }

            var bid = new Bid
            {
                AuctionId = auctionId,
                Bidder = User.Identity.Name,
                Amount = amount,
            };

            if(auction.AuctionEnd < DateTime.UtcNow)
            {
                bid.BisStatus = BidStatus.Finished;
            } else
            {
                var highBid = await DB.Find<Bid>()
                .Match(b => b.AuctionId == auctionId)
                .Sort(b => b.Descending(x => x.Amount))
                .ExecuteFirstAsync();

                if (highBid != null && amount > highBid.Amount || highBid == null)
                {
                    bid.BisStatus = amount > auction.ReservePrice ?
                        BidStatus.Accepted : BidStatus.AcceptedBelowReserve;
                }

                if (highBid != null && bid.Amount <= highBid.Amount)
                {
                    bid.BisStatus = BidStatus.TooLow;
                }
            }
            

            await DB.SaveAsync(bid);

            return Ok(bid);
        }
    }
}
