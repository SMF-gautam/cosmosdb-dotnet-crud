using CosmosDB.Entities;
using CosmosDB.IService;
using Microsoft.AspNetCore.Mvc;

namespace CosmosDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController(ICosmosDbService cosmosService) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(string id)
        {
            var item = await cosmosService.GetItemAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetAllItems(
            [FromQuery] string? continuationToken = null,
            [FromQuery] int pageSize = 10)
        {
            var (items, nextToken) = await cosmosService.GetItemsAsync("SELECT * FROM Items", continuationToken, pageSize);

            // Include continuation token in response headers
            if (!string.IsNullOrEmpty(nextToken))
                Response.Headers.Append("x-continuation-token", nextToken);

            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<Item>> CreateItem(Item newItem)
        {
            await cosmosService.AddItemAsync(newItem);
            return CreatedAtAction(nameof(GetItem), new { id = newItem.Id }, newItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(string id, Item updatedItem)
        {
            await cosmosService.UpdateItemAsync(id, updatedItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(string id)
        {
            await cosmosService.DeleteItemAsync(id);
            return NoContent();
        }
    }
}
