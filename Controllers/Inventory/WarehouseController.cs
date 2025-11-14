using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP_System_Project.Models.Inventory;
using ERP_System_Project.Models.Core;
using System.Linq;
using System.Threading.Tasks;
using System;
using ERP_System_Project.Models;

namespace ERP_System_Project.Controllers.Inventory
{
    public class WarehouseController : Controller
    {
        private readonly Erpdbcontext _context;
        public WarehouseController(Erpdbcontext context)
        {
            _context = context;
        }

        // GET: Index
        public async Task<IActionResult> Index()
        {
            var warehouses = await _context.Warehouses
                .Include(w => w.Branch)
                .Include(w => w.WarehouseProducts)
                    .ThenInclude(p => p.Product)
                .ToListAsync();
            return View(warehouses);
        }

        // GET: Details
        public async Task<IActionResult> Details(int id)
        {
            var warehouse = await _context.Warehouses
                .Include(w => w.Branch)
                .Include(w => w.WarehouseProducts)
                    .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(w => w.WarehouseId == id);

            if (warehouse == null) return NotFound();
            return PartialView(warehouse);
        }

        // GET: Create
        // GET: Create
        public IActionResult Create()
        {
            ViewData["Branches"] = new SelectList(_context.Branches, "Id", "Name");
            return PartialView();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                warehouse.CreatedDate = DateTime.Now;
                warehouse.IsActive = true;

                _context.Add(warehouse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Branches"] = new SelectList(_context.Branches, "Id", "Name", warehouse.BranchId);
            return View(warehouse);
        }

        // GET: Edit
        public async Task<IActionResult> Edit(int id)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse == null) return NotFound();

            ViewData["Branches"] = new SelectList(_context.Branches, "Id", "Name", warehouse.BranchId);
            return PartialView(warehouse);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Warehouse warehouse)
        {
            if (id != warehouse.WarehouseId) return NotFound();

            if (ModelState.IsValid)
            {
                warehouse.CreatedDate = warehouse.CreatedDate; // الحفاظ على تاريخ الإنشاء
                _context.Update(warehouse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Branches"] = new SelectList(_context.Branches, "Id", "Name", warehouse.BranchId);
            return View(warehouse);
        }


        // GET: Delete
        public async Task<IActionResult> Delete(int id)
        {
            var warehouse = await _context.Warehouses
                .Include(w => w.Branch)
                .Include(w => w.WarehouseProducts)
                    .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(w => w.WarehouseId == id);

            if (warehouse == null) return NotFound();
            return PartialView(warehouse);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var warehouse = await _context.Warehouses
                .Include(w => w.WarehouseProducts)
                .FirstOrDefaultAsync(w => w.WarehouseId == id);

            if (warehouse != null)
            {
                if (warehouse.WarehouseProducts.Any())
                {
                    ViewBag.ErrorMessage = "Cannot delete warehouse with existing products.";
                    return View(warehouse);
                }

                _context.Warehouses.Remove(warehouse);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
