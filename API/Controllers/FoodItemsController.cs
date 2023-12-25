using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

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
                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(ExcelConnection.InputFile);
                Excel._Worksheet xlWorksheet = xlWorkbook.Worksheets[1];
                Excel.Range xlRange = xlWorksheet.UsedRange;

                int rowCount = xlRange.Rows.Count;
                var seedData = new List<FoodItem>();
                for (int i = 2; i <= rowCount; i++)
                {
                    int id = (int)xlWorksheet.Cells[i, 1].value;
                    string name = (string)xlWorksheet.Cells[i, 2].value;
                    bool isComplete = (bool)xlWorksheet.Cells[i, 3].value;
                    string image = (string)xlWorksheet.Cells[i, 4].value;
                    float price = (float)xlWorksheet.Cells[i, 5].value;
                    string type = (string)xlWorksheet.Cells[i, 6].value;
                    FoodItem item = new FoodItem { Id = id, Name = name, IsComplete = isComplete, Image = image, Price = price, Type = type };
                    seedData.Add(item);
                }
                _context.AddRange(seedData);
                _context.SaveChanges();
                xlWorkbook.Close();
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
