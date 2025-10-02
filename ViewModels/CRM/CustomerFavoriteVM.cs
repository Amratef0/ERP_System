using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.ViewModels.CRM
{
    public class CustomerFavoriteVM
    {
        public int Id { get; set; }

       
        public int ProductId { get; set; }

        public string? ProductName { get; set; }
         public int CustomerId { get; set; }

        public string? CustomerFullName { get; set; }

        [Display(Name = "Date Added")]
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
