using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP_System_Project.Models;
using ERP_System_Project.Models.Inventory;

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
        public async Task<IActionResult> Create(int SupplierId, int ProductId, int WarehouseId, decimal Quantity, string PoNumber, DateTime OrderDate)
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
                PaymentTermsId = 1
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
                Notes = $"Purchase Order: {PoNumber}"
            };
            _context.InventoryTransactions.Add(inventoryTransaction);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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
