using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodItemsController : ControllerBase
    {
        private readonly FoodContext _context;

        public FoodItemsController(FoodContext context)
        {
            _context = context;
            //Seeding the database
            if (_context.FoodItems.Count() == 0)
            {
                var seedData = new FoodItem[]
                {
                    new FoodItem{ Id = 1, Name = "Peppino's Pizza", IsComplete = true, Image = "pizzafull.png", Price = 100, Type = "Food"},
                    new FoodItem{ Id = 2, Name = "Gustavo's Pizza", IsComplete = true, Image = "pizzafull.png", Price = 150, Type = "Food"}
                };
                _context.AddRange(seedData);
                _context.SaveChanges();
            }
        }

        // GET: api/FoodItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodItem>>> GetFoodItems()
        {
          if (_context.FoodItems == null)
          {
              return NotFound();
          }
            return await _context.FoodItems.ToListAsync();
        }

        // GET: api/FoodItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FoodItem>> GetFoodItem(long id)
        {
          if (_context.FoodItems == null)
          {
              return NotFound();
          }
            var foodItem = await _context.FoodItems.FindAsync(id);

            if (foodItem == null)
            {
                return NotFound();
            }

            return foodItem;
        }

        // PUT: api/FoodItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFoodItem(long id, FoodItem foodItem)
        {
            if (id != foodItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(foodItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/FoodItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FoodItem>> PostFoodItem(FoodItem foodItem)
        {
            _context.FoodItems.Add(foodItem);
            await _context.SaveChangesAsync();

            //    return CreatedAtAction("GetOrderItem", new { id = OrderItem.Id }, OrderItem);
            return CreatedAtAction(nameof(GetFoodItem), new { id = foodItem.Id }, foodItem);
        }

        [HttpPost("~/InsertMultiple")]
        public async Task<ActionResult<FoodItem>> PostFoodItem(List<FoodItem> foodItems)
        {
            _context.AddRange(foodItems);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFoodItems), new { foodItems });
        }

        // DELETE: api/FoodItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFoodItem(long id)
        {
            if (_context.FoodItems == null)
            {
                return NotFound();
            }
            var foodItem = await _context.FoodItems.FindAsync(id);
            if (foodItem == null)
            {
                return NotFound();
            }

            _context.FoodItems.Remove(foodItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FoodItemExists(long id)
        {
            return (_context.FoodItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
