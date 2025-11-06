using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP_System_Project.Models;
using System.Threading.Tasks;
using System.Linq;

namespace ERP_System_Project.Controllers
{
    public class SupplierCategoryController : Controller
    {
        private readonly Erpdbcontext _context;

        public SupplierCategoryController(Erpdbcontext context)
        {
            _context = context;
        }

        // GET: Supplier Categories
        public async Task<IActionResult> Index()
        {
            var categories = await _context.SupplierCategories
                .Include(c => c.Suppliers)
                .ToListAsync();
            return View(categories);
        }

        // GET: Details
        public async Task<IActionResult> Details(int id)
        {
            var category = await _context.SupplierCategories
                .Include(c => c.Suppliers)
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null) return NotFound();

            return PartialView(category);
        }

        // GET: Create
        [HttpGet]

        public IActionResult Create()
        {
            return PartialView();
        }

        // POST: Create
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(SupplierCategory category) // استخدم نفس اسم Action مع View
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.SupplierCategories.FindAsync(id);
            if (category == null) return NotFound();

            return PartialView(category);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SupplierCategory category)
        {
            if (id != category.CategoryId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.SupplierCategories.Any(c => c.CategoryId == category.CategoryId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Delete
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.SupplierCategories
                .Include(c => c.Suppliers)
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null) return NotFound();

            return PartialView(category);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.SupplierCategories.FindAsync(id);
            if (category != null)
            {
                _context.SupplierCategories.Remove(category);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
