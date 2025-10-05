namespace ERP_System_Project.Models.Enums
{
    public enum TimeActionStatus
    {
        Instant,      // < 1 ms
        Fast,         // 1 - 100 ms
        Moderate,     // 101 - 500 ms
        Slow,         // 501 - 1000 ms 
        VerySlow,     // > 1000 ms
    }
}
