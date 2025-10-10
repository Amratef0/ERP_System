using AutoMapper;
using ERP_System_Project.Extensions;
using ERP_System_Project.Helpers;
using ERP_System_Project.Models.HR;
using ERP_System_Project.Models.Inventory;
using ERP_System_Project.Services.Interfaces.HR;
using ERP_System_Project.UOW;
using ERP_System_Project.ViewModels.HR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ERP_System_Project.Services.Implementation.HR
{
    public class EmployeeService : GenericService<Employee>, IEmployeeService
    {
        private readonly IWebHostEnvironment env;
        private readonly IMapper mapper;

        public EmployeeService(IUnitOfWork uow, IWebHostEnvironment env, IMapper mapper) : base(uow)
        {
            this.env = env;
            this.mapper = mapper;
        }

        public async Task<bool> CreateAsync(EmployeeVM model)
        {
            var imageUrl = await model.Image.SaveImageAsync(env, "Uploads/Images/Employees");
            Employee employee = mapper.Map<Employee>(model);
            employee.ImageURL = imageUrl;
            employee.ModifiedDate = DateTime.UtcNow;

            try
            {
                await _uow.Employees.AddAsync(employee);
                await _uow.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(EmployeeVM model)
        {
            Employee employee = await _repository.GetByIdAsync(model.Id);

            if (employee == null) return false;

            if (model.RemoveImage && !string.IsNullOrEmpty(model.ImageURL))
            {
                await FileHelper.DeleteImageFileAsync(model.ImageURL);
                model.ImageURL = null;
            }

            if (model.NewImage != null)
            {
                if (!string.IsNullOrEmpty(model.ImageURL))
                {
                    await FileHelper.DeleteImageFileAsync(model.ImageURL);
                }
                model.ImageURL = await model.NewImage.SaveImageAsync(env, "Uploads/Images/Employees");
            }

            mapper.Map(model, employee);
            employee.ModifiedDate = DateTime.UtcNow;

            try
            {
                _repository.Update(employee);
                await _uow.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> IsExistAsync(int id)
        {
            return await _repository.IsExistAsync(id);
        }

        public async Task<Employee> GetByIdWithDetailsAsync(int id)
        {
            var query = _repository.GetAllAsIQueryable();

            var employee = query.Include(e => e.Address)
                                .ThenInclude(e => e.Country)
                                .Include(e => e.Department)
                                .Include(e => e.Branch)
                                .Include(e => e.Type)
                                .Include(e => e.JobTitle)
                                .Include(e => e.SalaryCurrency)
                                .FirstOrDefault(e => e.Id == id);

            return employee;
        }
        public new async Task<IEnumerable<EmployeeIndexVM>> GetAllAsync()
        {
            IEnumerable<EmployeeIndexVM> employees = await _repository.GetAllAsync(
                selector: e => new EmployeeIndexVM
                {
                    Id = e.Id,
                    FullName = $"{e.FirstName} {e.LastName}",
                    Branch = e.Branch,
                    Department = e.Department,
                    Type = e.Type,
                    JobTitle = e.JobTitle,
                });

            return employees;
        }

        public async Task<IEnumerable<EmployeeIndexVM>> SearchAsync(string? name, int? branchId, int? departmentId, int? employeeTypeId, int? jobTitleId)
        {
            var query = _repository.GetAllAsIQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => (e.FirstName + " " + e.LastName).ToUpper().StartsWith(name.ToUpper()));
            }

            if (branchId.HasValue && branchId.Value != 0)
            {
                query = query.Where(e => e.BranchId == branchId.Value);
            }

            if (departmentId.HasValue && departmentId.Value != 0)
            {
                query = query.Where(e => e.DepartmentId == departmentId.Value);
            }

            if (employeeTypeId.HasValue && employeeTypeId.Value != 0)
            {
                query = query.Where(e => e.TypeId == employeeTypeId.Value);
            }

            if (jobTitleId.HasValue && jobTitleId.Value != 0)
            {
                query = query.Where(e => e.JobTitleId == jobTitleId.Value);
            }

            IEnumerable<EmployeeIndexVM> employees = await query.Select(
                e => new EmployeeIndexVM
                {
                    Id = e.Id,
                    FullName = $"{e.FirstName} {e.LastName}",
                    Branch = e.Branch,
                    Department = e.Department,
                    Type = e.Type,
                    JobTitle = e.JobTitle,
                }).ToListAsync();

            return employees;
        }

    }
}
