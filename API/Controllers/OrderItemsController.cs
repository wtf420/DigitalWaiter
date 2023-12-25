using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly OrderContext _context;

        public OrderItemsController(OrderContext context)
        {
            _context = context;
        }

        private async void SyncData()
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(ExcelConnection.OutputFile);
            Excel._Worksheet xlWorksheet = xlWorkbook.Worksheets[1];

            List<OrderItem> items = await _context.OrderItems.ToListAsync();
            int rowindex = 2;
            foreach (OrderItem item in items)
            {
                xlWorksheet.Cells[rowindex, 1].value = item.Id;
                xlWorksheet.Cells[rowindex, 2].value = item.ExtraNote;
                xlWorksheet.Cells[rowindex, 3].value = item.Completed;
                xlWorksheet.Cells[rowindex, 4].value = item.Price;
                xlWorksheet.Cells[rowindex, 5].value = item.Date;
                if (item.OrderFoodItems != null)
                {
                    string str = "";
                    foreach (FoodItem fooditem in item.OrderFoodItems)
                    {
                        str += fooditem.Name + "\n";
                    }
                    xlWorksheet.Cells[rowindex, 6].value = str;
                }
                else
                    xlWorksheet.Cells[rowindex, 6].value = "null";
            }
            xlWorkbook.Save();
            xlWorkbook.Close();
        }

        // GET: api/OrderItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems()
        {

            if (_context.OrderItems == null)
            {
                return NotFound();
            }
            return await _context.OrderItems.Include(x => x.OrderFoodItems).ToListAsync();
        }

        // GET: api/OrderItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItem>> GetOrderItem(long id)
        {
          if (_context.OrderItems == null)
          {
              return NotFound();
          }
            var orderItem = await _context.OrderItems.FindAsync(id);

            if (orderItem == null)
            {
                return NotFound();
            }

            return orderItem;
        }

        // PUT: api/OrderItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderItem(long id, OrderItem orderItem)
        {
            if (id != orderItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(orderItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderItemExists(id))
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

        // POST: api/OrderItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderItem>> PostOrderItem(OrderItem orderItem)
        {
            if (_context.OrderItems == null)
            {
                return Problem("Entity set 'OrderContext.OrderItems'  is null.");
            }

            orderItem.Id = _context.OrderItems.Count() + 1;
            foreach (FoodItem foodItem in orderItem.OrderFoodItems)
            {
                foodItem.Id = 0;
            }
            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();
            await Task.Run(() => SyncData());

            return CreatedAtAction(nameof(GetOrderItem), new { id = orderItem.Id }, orderItem);
        }

        // DELETE: api/OrderItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(long id)
        {
            if (_context.OrderItems == null)
            {
                return NotFound();
            }
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }

            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderItemExists(long id)
        {
            return (_context.OrderItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
