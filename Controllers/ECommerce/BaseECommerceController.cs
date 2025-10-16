using Microsoft.AspNetCore.Mvc;

namespace ERP_System_Project.Controllers.ECommerce
{
    public class BaseECommerceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
