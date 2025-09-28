using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.HR;
using ERP_System_Project.UOW;

namespace ERP_System_Project.Services.Implementation.HR
{
    public class WorkScheduleDayService : GenericService<WorkScheduleDay>, IWorkScheduleDayService
    {
        public WorkScheduleDayService(IUnitOfWork uow) : base(uow) { }
    }
}
