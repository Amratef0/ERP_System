using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP_System_Project.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ERP_System_Project.Controllers
{
    public class SupplierController : Controller
    {
        private readonly Erpdbcontext _context;

        public SupplierController(Erpdbcontext context)
        {
            _context = context;
        }

        // GET: Suppliers
        public async Task<IActionResult> Index()
        {
            var suppliers = await _context.Suppliers
                .Where(s => s.IsActive)
                .Include(s => s.SupplierCategory)
                .ToListAsync();

            return View(suppliers);
        }

        // GET: Supplier Details
        public async Task<IActionResult> Details(int id)
        {
            var supplier = await _context.Suppliers
                .Include(s => s.SupplierCategory)
                .Include(s => s.SupplierProducts)
                    .ThenInclude(sp => sp.Product)
                .Include(s => s.PurchaseOrders)
                .FirstOrDefaultAsync(s => s.SupplierId == id && s.IsActive);

            if (supplier == null)
                return NotFound();

            return View(supplier);
        }

        // GET: Create Supplier
        // GET: Create Supplier
        public IActionResult Create()
        {
            ViewData["SupplierCategories"] = new SelectList(_context.SupplierCategories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Create Supplier
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                supplier.CreatedDate = DateTime.Now;
                supplier.ModifiedDate = DateTime.Now;
                supplier.IsActive = true;

                _context.Add(supplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["SupplierCategories"] = new SelectList(_context.SupplierCategories, "CategoryId", "CategoryName", supplier.SupplierCategoryId);
            return View(supplier);
        }

        // GET: Edit Supplier
        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null || !supplier.IsActive) return NotFound();

            ViewData["SupplierCategories"] = new SelectList(_context.SupplierCategories, "CategoryId", "CategoryName", supplier.SupplierCategoryId);
            return View(supplier);
        }

        // POST: Edit Supplier
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Supplier supplier)
        {
            if (id != supplier.SupplierId) return NotFound();

            if (ModelState.IsValid)
            {
                supplier.ModifiedDate = DateTime.Now;
                _context.Update(supplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["SupplierCategories"] = new SelectList(_context.SupplierCategories, "CategoryId", "CategoryName", supplier.SupplierCategoryId);
            return View(supplier);
        }


        // GET: Delete Supplier
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier != null)
            {
                supplier.IsActive = false;
                supplier.ModifiedDate = DateTime.Now;
                _context.Update(supplier);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
