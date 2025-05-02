using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Entities;

namespace SearchService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        //searching items with optional search term and pagination.
        [HttpGet]
        public async Task<ActionResult<List<Item>>> SearchItems(string? searchTerm, int pageNumber = 1, int pageSize = 4)
        {
            // Create a paged search query for the Item collection
            var query = DB.PagedSearch<Item>();
            query.Sort(x => x.Ascending(a => a.Make));
            // If a search term is provided, apply full-text search and sort results by relevance score
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query.Match(Search.Full, searchTerm).SortByTextScore();
            }
            // Apply pagination
            query.PageNumber(pageNumber);
            query.PageSize(pageSize);

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
