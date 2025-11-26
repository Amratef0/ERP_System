using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP_System_Project.Models;
using ERP_System_Project.Models.Inventory;
using static Azure.Core.HttpHeader;

namespace ERP_System_Project.Controllers
{
    public class PurchaseOrderController : Controller
    {
        private readonly Erpdbcontext _context;

        public PurchaseOrderController(Erpdbcontext context)
        {
            _context = context;
        }

        // GET: PurchaseOrder/Create
        public IActionResult Create(int? supplierId)
        {
            // كل الـ Suppliers
            ViewData["Suppliers"] = new SelectList(_context.Suppliers.Where(s => s.IsActive), "SupplierId", "SupplierName", supplierId);

            // المنتجات حسب الـ Supplier المحدد
            if (supplierId.HasValue)
            {
                var products = _context.SupplierProducts
                    .Include(sp => sp.Product)
                    .Where(sp => sp.SupplierId == supplierId.Value && sp.IsActive)
                    .Select(sp => new { sp.ProductId, sp.Product!.Name })
                    .ToList();

                ViewData["Products"] = new SelectList(products, "ProductId", "Name");
            }
            else
            {
                ViewData["Products"] = new SelectList(Enumerable.Empty<Product>(), "ProductId", "Name");
            }

            // Warehouses
            ViewData["Warehouses"] = new SelectList(_context.Warehouses.Where(w => w.IsActive), "WarehouseId", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int SupplierId, int ProductId, int WarehouseId, decimal Quantity, string PoNumber, DateTime OrderDate, string Notes)
        {
            if (SupplierId == 0 || ProductId == 0 || WarehouseId == 0 || Quantity <= 0)
            {
                ModelState.AddModelError("", "Please fill all required fields correctly.");
                return RedirectToAction(nameof(Create), new { supplierId = SupplierId });
            }

            // احضار أو إنشاء WarehouseProduct
            var warehouseProduct = await _context.WarehouseProducts
                .FirstOrDefaultAsync(wp => wp.ProductId == ProductId && wp.WarehouseId == WarehouseId);

            if (warehouseProduct == null)
            {
                warehouseProduct = new WarehouseProduct
                {
                    ProductId = ProductId,
                    WarehouseId = WarehouseId,
                    QuantityOnHand = Quantity
                };
                _context.WarehouseProducts.Add(warehouseProduct);
                await _context.SaveChangesAsync(); // حفظ لتوليد Id
            }
            else
            {
                warehouseProduct.QuantityOnHand += Quantity;
                _context.WarehouseProducts.Update(warehouseProduct);
                await _context.SaveChangesAsync();
            }

            // تحديث Quantity في جدول Product الحقيقي
            var product = await _context.Products.FindAsync(ProductId);
            if (product != null)
            {
                product.Quantity += (int)Quantity;
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
            }

            // إضافة الـ PurchaseOrder
            var purchaseOrder = new PurchaseOrder
            {
                SupplierId = SupplierId,
                WarehouseProductId = warehouseProduct.Id,
                WarehouseId = WarehouseId,
                Quantity = Quantity,
                PoNumber = PoNumber,
                OrderDate = OrderDate,
                StatusId = 1,
                PaymentTermsId = 1,
                Notes = Notes
            };

            _context.PurchaseOrders.Add(purchaseOrder);
            await _context.SaveChangesAsync();

            // إنشاء سجل في InventoryTransaction
            var inventoryTransaction = new InventoryTransaction
            {
                ProductId = ProductId,
                WarehouseProductId = warehouseProduct.Id,
                Quantity = Quantity,
                TransactionType = "Purchase",
                TransactionDate = DateTime.Now,
                Notes = Notes
            };
            _context.InventoryTransactions.Add(inventoryTransaction);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }






        // GET: PurchaseOrder/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var order = await _context.PurchaseOrders
                .Include(po => po.WarehouseProduct)
                .ThenInclude(wp => wp.Product)
                .FirstOrDefaultAsync(po => po.PurchaseOrderId == id);

            if (order == null)
                return NotFound();

            // Suppliers
            ViewData["Suppliers"] = new SelectList(_context.Suppliers.Where(s => s.IsActive), "SupplierId", "SupplierName", order.SupplierId);

            // Warehouses
            ViewData["Warehouses"] = new SelectList(_context.Warehouses.Where(w => w.IsActive), "WarehouseId", "Name", order.WarehouseId);

            // Products (based on Supplier)
            var products = _context.SupplierProducts
                .Include(sp => sp.Product)
                .Where(sp => sp.SupplierId == order.SupplierId && sp.IsActive)
                .Select(sp => new { sp.ProductId, sp.Product!.Name })
                .ToList();

            ViewData["Products"] = new SelectList(products, "ProductId", "Name", order.WarehouseProduct?.ProductId);

            return View(order);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PurchaseOrder updatedOrder)
        {
            if (id != updatedOrder.PurchaseOrderId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    updatedOrder.ModifiedDate = DateTime.Now;
                    _context.Update(updatedOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.PurchaseOrders.Any(e => e.PurchaseOrderId == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(updatedOrder);
        }

        // GET: PurchaseOrder/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var order = await _context.PurchaseOrders
                .Include(po => po.Supplier)
                .FirstOrDefaultAsync(po => po.PurchaseOrderId == id);

            if (order == null)
                return NotFound();

            return PartialView(order);
        }

        // POST: PurchaseOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.PurchaseOrders.FindAsync(id);
            if (order != null)
            {
                _context.PurchaseOrders.Remove(order);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }





        [HttpGet]
        public IActionResult GetSupplierProducts(int supplierId)
        {
            var products = _context.SupplierProducts
                .Include(sp => sp.Product)
                .Where(sp => sp.SupplierId == supplierId && sp.IsActive)
                .Select(sp => new
                {
                    productId = sp.ProductId,
                    name = sp.Product!.Name
                })
                .ToList();

            return Json(products);
        }

        // GET: PurchaseOrder/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var order = await _context.PurchaseOrders
                .Include(po => po.Supplier)
                .Include(po => po.Warehouse)
                .Include(po => po.WarehouseProduct)
                    .ThenInclude(wp => wp.Product)
                .FirstOrDefaultAsync(po => po.PurchaseOrderId == id);

            if (order == null)
                return NotFound();

            return View(order);
        }


        // GET: PurchaseOrder
        public async Task<IActionResult> Index()
        {
            var orders = await _context.PurchaseOrders
                .Include(po => po.Supplier)
                .Include(po => po.Warehouse)
                .Include(po => po.WarehouseProduct)
                    .ThenInclude(wp => wp.Product)
                .ToListAsync();

            return View(orders);
        }
    }
}
