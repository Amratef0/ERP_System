using ERP_System_Project.Models.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP_System_Project.Services.Interfaces.System_Security
{
    public interface IUserService
    {
        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser?> GetUserByIdAsync(string id);
        Task<bool> CreateUserAsync(ApplicationUser user, string password);
        Task<bool> UpdateUserAsync(ApplicationUser user);
        Task<bool> DeleteUserAsync(string id);
        Task<bool> SetActiveStatusAsync(string id, bool isActive);
        Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
    }
}
