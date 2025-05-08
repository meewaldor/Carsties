using AutoMapper;
using BiddingService.DTOs;
using BiddingService.Models;

namespace BiddingService.RequestHelpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Bid, BidDto>();
        }
    }
}
