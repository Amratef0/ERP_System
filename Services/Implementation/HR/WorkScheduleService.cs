using AutoMapper;
using ERP_System_Project.Models.Enums;
using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.HR;
using ERP_System_Project.UOW;
using ERP_System_Project.ViewModels.HR;

namespace ERP_System_Project.Services.Implementation.HR
{
    public class WorkScheduleService : GenericService<WorkSchedule>, IWorkScheduleService
    {
        public WorkScheduleService(IUnitOfWork uow, IMapper mapper) : base(uow)
        {
        }

        public async Task<ICollection<WorkScheduleDay>?> GetScheduleDaysByIdAsync(int workScheduleId)
            => await _repository.GetAsync(ws => ws.ScheduleDays, ws => ws.Id == workScheduleId);

        public async Task<WorkScheduleDay?> GetScheduleDayByIdAsync(int workScheduleId, int dayId)
        {
            ICollection<WorkScheduleDay>? days = await GetScheduleDaysByIdAsync(workScheduleId);
            WorkScheduleDay? day = days?.FirstOrDefault(d => d.Id == dayId);
            return day;
        }

        public async Task<bool> CheckIfDayOffAsync(DateOnly date, int workScheduleId)
        {
            IEnumerable<WorkScheduleDay> days = await _repository.GetAsync(ws => ws.ScheduleDays, ws => ws.Id == workScheduleId);
            bool isDayOff = days.Any(d => d.Day == date.DayOfWeek && !d.IsWorkDay);
            return isDayOff;
        }

        public async Task<bool> SoftDelete(int id)
        {
            WorkSchedule? entity = await _repository.GetByIdAsync(id);
            if (entity == null) return false;

            _repository.SoftDelete(id);
            await _uow.CompleteAsync();
            return true;
        }
    }
}
