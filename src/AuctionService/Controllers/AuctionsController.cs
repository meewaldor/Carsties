using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionsController : ControllerBase
    {
        private readonly AuctionDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        public AuctionsController(AuctionDbContext context, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }
        // API endpoint that returns a list of AuctionDto objects.
        // If a date is provided, only auctions updated after that date are returned.
        [HttpGet]
        public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions(string? date)
        {
            var query = _context.Auctions.OrderBy(x => x.Item.Make).AsQueryable();
            if ((!string.IsNullOrEmpty(date)))
            {
                query = query.Where(x => x.UpdatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
            }
            return await query.ProjectTo<AuctionDto>(_mapper.ConfigurationProvider).ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
        {
            var auction = await _context.Auctions
                .Include(x => x.Item)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (auction == null) return NotFound();
            return _mapper.Map<AuctionDto>(auction);
        }
        [HttpPost]
        public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctionDto)
        {
            var auction = _mapper.Map<Auction>(auctionDto);
            auction.Seller = "test";
            _context.Auctions.Add(auction);
            
            var newAuction = _mapper.Map<AuctionDto>(auction);
            // publish message to queue
            await _publishEndpoint.Publish(_mapper.Map<AuctionCreated>(newAuction));
            
            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return BadRequest("Failed to create auction");
            return CreatedAtAction(nameof(GetAuctionById), new { id = auction.Id }, _mapper.Map<AuctionDto>(auction));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
        {
            var auction = await _context.Auctions
                .Include(x => x.Item)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (auction == null) return NotFound();
            
            // check seller = username

            auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
            auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
            auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
            auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
            auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;

            var result = await _context.SaveChangesAsync() > 0;
            if (result) return Ok();
            return BadRequest("Failed to update auction");
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAuction(Guid id)
        {
            var auction = await _context.Auctions.FindAsync(id);
            if (auction == null) return NotFound();
            // check seller = username

            _context.Auctions.Remove(auction);
            var result = await _context.SaveChangesAsync() > 0;
            if (result) return Ok();
            return BadRequest("Failed to delete auction");
        }
    }
}
