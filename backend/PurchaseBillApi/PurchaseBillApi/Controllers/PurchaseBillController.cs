using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PurchaseBillApi.Data;
using PurchaseBillApi.DTOs;

namespace PurchaseBillApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PurchaseBillController : ControllerBase
    {
        [HttpPost("submit")]
        public ActionResult<PurchaseBillSubmitResultDto> Submit([FromBody] PurchaseBillSubmitRequestDto request)
        {
            if (request.Items == null || request.Items.Count == 0)
            {
                return BadRequest(new PurchaseBillSubmitResultDto
                {
                    Success = false,
                    Message = "At least one item is required."
                });
            }

            foreach (var item in request.Items)
            {
                if (string.IsNullOrWhiteSpace(item.Item) || string.IsNullOrWhiteSpace(item.Batch))
                {
                    return BadRequest(new PurchaseBillSubmitResultDto
                    {
                        Success = false,
                        Message = "Each item must have an Item name and Batch selected."
                    });
                }

                if (item.Quantity <= 0)
                {
                    return BadRequest(new PurchaseBillSubmitResultDto
                    {
                        Success = false,
                        Message = $"Quantity must be greater than zero for item '{item.Item}'."
                    });
                }

                // Recompute server-side — never trust client-calculated totals
                var expectedTotalCost = (item.StandardCost * item.Quantity) * (1 - (item.Discount / 100m));
                var expectedTotalSelling = item.StandardPrice * item.Quantity;

                if (Math.Abs(expectedTotalCost - item.TotalCost) > 0.01m ||
                    Math.Abs(expectedTotalSelling - item.TotalSelling) > 0.01m)
                {
                    return BadRequest(new PurchaseBillSubmitResultDto
                    {
                        Success = false,
                        Message = $"Calculated totals for '{item.Item}' do not match expected values."
                    });
                }
            }

            return Ok(new PurchaseBillSubmitResultDto
            {
                Success = true,
                Message = "Purchase bill submitted successfully.",
                TotalItems = request.Items.Count,
                TotalQuantity = request.Items.Sum(i => i.Quantity)
            });
        }

        [HttpGet("locations")]
        public async Task<ActionResult<List<string>>> GetLocations([FromServices] AppDbContext dbContext)
        {
            var locationNames = await dbContext.Location_Details
                .Select(l => l.Location_Name)
                .Distinct()
                .OrderBy(name => name)
                .ToListAsync();

            return Ok(locationNames);
        }
    }
}
