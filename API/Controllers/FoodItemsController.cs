using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;

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
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage package = new ExcelPackage(ExcelConnection.InputFile);
                ExcelWorksheet xlWorksheet = package.Workbook.Worksheets[0];

                int rowCount = xlWorksheet.Rows.Count();
                var seedData = new List<FoodItem>();
                var k = xlWorksheet.Drawings;
                for (int i = 2; i < rowCount; i++)
                {
                    long id = Convert.ToInt32(xlWorksheet.Cells[i, 1].Value);
                    string name = (string)xlWorksheet.Cells[i, 2].Value;
                    string description = (string)xlWorksheet.Cells[i, 3].Value;
                    string includedItems = (string)xlWorksheet.Cells[i, 4].Value;
                    bool IsAvailable = (bool)xlWorksheet.Cells[i, 5].Value;
                    double price = (double)xlWorksheet.Cells[i, 7].Value;
                    string type = (string)xlWorksheet.Cells[i, 8].Value;

                    string picture = null;
                    string? path = (string)xlWorksheet.Cells[i, 6].Value;
                    using (Image image = Image.FromFile(path))
                    {
                        using (MemoryStream m = new MemoryStream())
                        {
                            image.Save(m, image.RawFormat);
                            byte[] imageBytes = m.ToArray();

                            // Convert byte[] to Base64 String
                            picture = Convert.ToBase64String(imageBytes);
                        }
                    }

                    FoodItem item = new FoodItem { Id = id, Name = name, Description = description, IncludedItems = includedItems, IsAvailable = IsAvailable, Image = picture, Price = price, Type = type };
                    seedData.Add(item);
                }

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
