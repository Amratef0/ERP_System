using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models.ECommerce
{
    public class TempCart
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public string CartDataJson { get; set; } // هنا هنخزن الكارت كله كـ JSON

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsProcessed { get; set; } = false; // عشان نعرف إذا تم عمل الأوردر
    }
}
