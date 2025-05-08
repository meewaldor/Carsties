using AutoMapper;
using BiddingService.DTOs;
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
        private readonly IMapper _mapper;
        public BidsController(IMapper mapper)
        {
            _mapper = mapper;
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BidDto>> PlaceBid(string auctionId, int amount)
        {
            var auction = await DB.Find<Auction>().OneAsync(auctionId);
            if (auction == null)
            {
                return NotFound("Auction not found");
            }
            if (auction.Seller == User.Identity.Name)
            {
                return BadRequest("You cannot bid on your own auction");
            }

            var bid = new Bid
            {
                AuctionId = auctionId,
                Bidder = User.Identity.Name,
                Amount = amount,
            };

            if (auction.AuctionEnd < DateTime.UtcNow)
            {
                bid.BidStatus = BidStatus.Finished;
            } else
            {
                var highBid = await DB.Find<Bid>()
                .Match(b => b.AuctionId == auctionId)
                .Sort(b => b.Descending(x => x.Amount))
                .ExecuteFirstAsync();

                if (highBid != null && amount > highBid.Amount || highBid == null)
                {
                    bid.BidStatus = amount > auction.ReservePrice ?
                        BidStatus.Accepted : BidStatus.AcceptedBelowReserve;
                }

                if (highBid != null && bid.Amount <= highBid.Amount)
                {
                    bid.BidStatus = BidStatus.TooLow;
                }
            }


            await DB.SaveAsync(bid);

            return Ok(_mapper.Map<BidDto>(bid));
        }
    

    [HttpGet("{auctionId}")]
        public async Task<ActionResult<List<BidDto>>> GetBids(string auctionId)
        {
            var bids = await DB.Find<Bid>()
                .Match(b => b.AuctionId == auctionId)
                .Sort(b => b.Descending(x => x.BidTime))
                .ExecuteAsync();
            return bids.Select(_mapper.Map<BidDto>).ToList();
        }
    }
}