using ERP_System_Project.Models.Core;
using ERP_System_Project.Models.HR;

namespace ERP_System_Project.ViewModels.HR
{
    public class EmployeesIndexVM
    {
        public IEnumerable<EmployeeIndexVM> Employees { get; set; }

        public IEnumerable<Branch> Branches { get; set; }

        public IEnumerable<Department> Departments { get; set; }

        public IEnumerable<EmployeeType> EmployeeTypes { get; set; }

        public IEnumerable<JobTitle> JobTitles { get; set; }
    }
}
