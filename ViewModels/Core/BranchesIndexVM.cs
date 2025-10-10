using ERP_System_Project.Models.Core;

namespace ERP_System_Project.ViewModels.Core
{
    public class BranchesIndexVM
    {
        public IEnumerable<Branch> Branches { get; set; }
        public IEnumerable<Country> Countries { get; set; }
    }
}
