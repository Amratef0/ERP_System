namespace ERP_System_Project.Models.Entites
{
    public class Brand
    {
        public int brand_Id { get; set; }
        public string brand_name { get; set; } = null!;
        public string brand_description { get; set; } = null!;
        public string logo_url { get; set; } = null!;
        public string website_url { get; set; }
        public bool is_active { get; set; } = true;
        public DateTime created_date { get; set; } = DateTime.Now;
    }
}
