using ERP_System_Project.Models.Enums;

namespace ERP_System_Project.Models.Logs
{
    public class PerformanceLog
    {
        public int Id { get; set; }
        public string EndPointName { get; set; } = null!;
        public DateTime RequestDate { get; set; } = DateTime.Now;
        public string ElabsedTime { get; set; } = null!;
        public TimeActionStatus Status { get; set; }
    }
}
