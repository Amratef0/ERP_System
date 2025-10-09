using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP_System_Project.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ERP_System_Project.Controllers
{
    public class SupplierProductController : Controller
    {
        private readonly Erpdbcontext _context;

        public SupplierProductController(Erpdbcontext context)
        {
            _context = context;
        }

        // GET: Index
        public async Task<IActionResult> Index()
        {
            var supplierProducts = await _context.SupplierProducts
                .Where(sp => sp.IsActive)
                .Include(sp => sp.Supplier)
                .Include(sp => sp.Product)
                .ToListAsync();
            return View(supplierProducts);
        }

        // GET: Details
        public async Task<IActionResult> Details(int id)
        {
            var sp = await _context.SupplierProducts
                .Include(s => s.Supplier)
                .Include(s => s.Product)
                .FirstOrDefaultAsync(s => s.SupplierProductId == id && s.IsActive);

            if (sp == null) return NotFound();
            return View(sp);
        }

        // GET: Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Suppliers = new SelectList(_context.Suppliers.Where(s => s.IsActive), "SupplierId", "SupplierName");
            ViewBag.Products = new SelectList(_context.Products.Where(p => p.IsActive), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupplierProduct model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.IsActive = true;

                _context.SupplierProducts.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Suppliers = new SelectList(_context.Suppliers, "SupplierId", "SupplierName", model.SupplierId);
            ViewBag.Products = new SelectList(_context.Products, "Id", "Name", model.ProductId);

            return View(model);
        }

        // GET: Edit
        public async Task<IActionResult> Edit(int id)
        {
            var sp = await _context.SupplierProducts.FindAsync(id);
            if (sp == null || !sp.IsActive) return NotFound();

            ViewBag.Suppliers = new SelectList(_context.Suppliers.Where(s => s.IsActive), "SupplierId", "SupplierName", sp.SupplierId);
            ViewBag.Products = new SelectList(_context.Products.Where(p => p.IsActive), "Id", "Name", sp.ProductId);
            return View(sp);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SupplierProduct sp)
        {
            if (id != sp.SupplierProductId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    sp.ModifiedDate = DateTime.Now;
                    _context.Update(sp);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.SupplierProducts.Any(s => s.SupplierProductId == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Suppliers = new SelectList(_context.Suppliers, "SupplierId", "SupplierName", sp.SupplierId);
            ViewBag.Products = new SelectList(_context.Products, "Id", "Name", sp.ProductId);
            return View(sp);
        }

        // GET: Delete
        public async Task<IActionResult> Delete(int id)
        {
            var sp = await _context.SupplierProducts
                .Include(s => s.Supplier)
                .Include(s => s.Product)
                .FirstOrDefaultAsync(s => s.SupplierProductId == id && s.IsActive);

            if (sp == null) return NotFound();
            return View(sp);
        }

        // POST: Delete (Soft Delete)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sp = await _context.SupplierProducts.FindAsync(id);
            if (sp != null)
            {
                sp.IsActive = false;
                sp.ModifiedDate = DateTime.Now;
                _context.Update(sp);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
