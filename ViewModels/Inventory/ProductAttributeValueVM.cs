using ERP_System_Project.Models.Inventory;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.ViewModels.Inventory
{
    public class ProductAttributeValueVM
    {
        public int AtrributeId { get; set; }
        public string Value { get; set; } = null!;
    }
}
