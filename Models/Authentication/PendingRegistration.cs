namespace ERP_System_Project.Models.Authentication
{
    public class PendingRegistration
    {
        public ApplicationUser User { get; set; }
        public string Token { get; set; }
    }

}