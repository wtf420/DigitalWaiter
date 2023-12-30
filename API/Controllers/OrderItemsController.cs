using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;
using System.Diagnostics;
using OfficeOpenXml;

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
            ExcelPackage package = new ExcelPackage(ExcelConnection.OutputFile);
            ExcelWorksheet xlWorksheet = package.Workbook.Worksheets[0];

            List<OrderItem> items = await _context.OrderItems.ToListAsync();
            int rowindex = xlWorksheet.Rows.Count() <= 1 ? 1 : xlWorksheet.Rows.Count();
            foreach (OrderItem item in items)
            {
                rowindex++;
                xlWorksheet.Cells[rowindex, 1].Value = item.Id;
                xlWorksheet.Cells[rowindex, 2].Value = item.ExtraNote;
                xlWorksheet.Cells[rowindex, 3].Value = item.Completed;
                xlWorksheet.Cells[rowindex, 4].Value = item.TotalPrice;
                xlWorksheet.Cells[rowindex, 5].Value = item.Date;
                if (item.PurchasedItems != null)
                {
                    string str = "";
                    foreach (PurchaseInfo purchaseInfo in item.PurchasedItems)
                    {
                        str += $"'{purchaseInfo.FoodItemId} x'{purchaseInfo.Quantity}'";
                    }
                    xlWorksheet.Cells[rowindex, 6].Value = str;
                }
                else
                    xlWorksheet.Cells[rowindex, 7].Value = "null";
                package.Save();
            }
        }

        // GET: api/OrderItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems()
        {
            if (_context.OrderItems == null)
            {
                return NotFound();
            }
            return await _context.OrderItems.Include(x => x.PurchasedItems).ToListAsync();
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
