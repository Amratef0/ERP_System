using ERP_System_Project.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models.HR
{
    public enum LeaveRequestStatus
    {
        Pending = 1,
        Approved,
        Rejected,
        Cancelled
    }
}
