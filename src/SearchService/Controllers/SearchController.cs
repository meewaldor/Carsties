using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Entities;
using SearchService.RequestHelpers;

namespace SearchService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        //searching items with optional search term and pagination.
        [HttpGet]
        public async Task<ActionResult<List<Item>>> SearchItems([FromQuery] SearchParams searchParams)
        {
            // Create a paged search query for the Item collection
            var query = DB.PagedSearch<Item, Item>();
            // If a search term is provided, apply full-text search and sort results by relevance score
            if (!string.IsNullOrEmpty(searchParams.SearchTerm))
            {
                query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
            }
            // Apply pagination
            query.PageNumber(searchParams.PageNumber);
            query.PageSize(searchParams.PageSize);

            // Dynamically apply sorting to the query based on the user's selected order option.
            query = searchParams.OrderBy switch
            {
                "make" => query.Sort(x => x.Ascending(a => a.Make))
                .Sort(x => x.Ascending(a => a.Model)),
                "new" => query.Sort(x => x.Descending(a => a.CreatedAt)),
                _ => query.Sort(x => x.Ascending(a => a.AuctionEnd))
            };

            query = searchParams.FilterBy switch
            {
                "finish" => query.Match(x => x.AuctionEnd < DateTime.UtcNow),
                "endingSoon" => query.Match(x => x.AuctionEnd < DateTime.UtcNow.AddHours(6) && x.AuctionEnd > DateTime.UtcNow),
                _ => query.Match(x => x.AuctionEnd > DateTime.UtcNow)
            };

            if (!string.IsNullOrEmpty(searchParams.Seller))
            {
                query.Match(x => x.Seller == searchParams.Seller);
            }
            if (!string.IsNullOrEmpty(searchParams.Winner))
            {
                query.Match(x => x.Winner == searchParams.Winner);
            }

            var result = await query.ExecuteAsync();
            return Ok(new
            {
                results = result.Results,
                pageCount = result.PageCount, // Total number of pages
                totalCount = result.TotalCount // Total number of matched items
            });
        }
    }
}
