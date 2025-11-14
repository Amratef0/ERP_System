using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP_System_Project.Models.Inventory;
using System.Threading.Tasks;
using System.Linq;
using ERP_System_Project.Models;

namespace ERP_System_Project.Controllers.Inventory
{
    public class InventoryTransactionController : Controller
    {
        private readonly Erpdbcontext _context;

        public InventoryTransactionController(Erpdbcontext context)
        {
            _context = context;
        }

        // GET: InventoryTransactions
        public async Task<IActionResult> Index()
        {
            var transactions = await _context.InventoryTransactions
                .Include(t => t.Product)
                .Include(t => t.WarehouseProduct)
                    .ThenInclude(wp => wp.Warehouse)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();

            return View(transactions);
        }

        // GET: Details
        public async Task<IActionResult> Details(int id)
        {
            var transaction = await _context.InventoryTransactions
                .Include(t => t.Product)
                .Include(t => t.WarehouseProduct)
                    .ThenInclude(wp => wp.Warehouse)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null) return NotFound();

            return PartialView(transaction);
        }
    }
}
