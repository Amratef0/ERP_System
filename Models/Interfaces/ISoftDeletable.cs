namespace ERP_System_Project.Models.Interfaces
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
        DateOnly? DeletedAt { get; set; }
    }
}
